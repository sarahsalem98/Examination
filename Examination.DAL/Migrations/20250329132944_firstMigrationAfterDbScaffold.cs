using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class firstMigrationAfterDbScaffold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_SubmitStudentAnswers')
            BEGIN
                EXEC('CREATE PROCEDURE Exam.sp_SubmitStudentAnswers
                    @StudentId INT,
                    @GeneratedExamId INT,
                    @StudentAnswers ExamAnswerType READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRANSACTION;
                    BEGIN TRY
                        INSERT INTO Exam_Student_Answers (StudentId, GeneratedExamQsId, Generated_Exam_Id, StdAnswer, SubmittedAt)
                        SELECT @StudentId, GeneratedExamQsId, @GeneratedExamId, StdAnswer, GETDATE()
                        FROM @StudentAnswers;
                        COMMIT TRANSACTION;
                        RETURN 1;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        RETURN -1;
                    END CATCH
                END');
            END");

            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_SubmitSingleStudentAnswer')
            BEGIN
                EXEC('CREATE PROCEDURE Exam.sp_SubmitSingleStudentAnswer
                    @StudentId INT,
                    @GeneratedExamId INT,
                    @GeneratedExamQsId INT,
                    @StdAnswer NVARCHAR(MAX)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRANSACTION;
                    BEGIN TRY
                        INSERT INTO Exam_Student_Answers (StudentId, GeneratedExamQsId, Generated_Exam_Id, StdAnswer, SubmittedAt)
                        VALUES (@StudentId, @GeneratedExamQsId, @GeneratedExamId, @StdAnswer, GETDATE());
                        COMMIT TRANSACTION;
                        RETURN 1;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        RETURN -1;
                    END CATCH
                END');
            END");

            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CorrectStudentAnswers')
            BEGIN
                EXEC('CREATE PROCEDURE Exam.sp_CorrectStudentAnswers
                    @StudentId INT,
                    @GeneratedExamId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @TotalExamGrade INT;
                    DECLARE @CorrectTotalDegree INT;
                    DECLARE @StudentDegreePercent INT;
                    DECLARE @result INT;

                    BEGIN TRANSACTION;
                    BEGIN TRY
                        SELECT @TotalExamGrade = Grade FROM Generated_Exams WHERE Id = @GeneratedExamId;
                        IF @TotalExamGrade = 0
                        BEGIN
                            ROLLBACK TRANSACTION;
                            RETURN -1;
                        END

                        SELECT @CorrectTotalDegree = SUM(EQ.Degree)
                        FROM Exam_Student_Answers ESA
                        JOIN Generated_Exam_Qs GEQ ON ESA.GeneratedExamQsId = GEQ.Id
                        JOIN Exam_Qs EQ ON GEQ.ExamQsId = EQ.Id
                        WHERE ESA.StudentId = @StudentId
                        AND ESA.Generated_Exam_Id = @GeneratedExamId
                        AND ESA.StdAnswer = EQ.RightAnswer;

                        SET @StudentDegreePercent = (@CorrectTotalDegree * 100) / @TotalExamGrade;

                        IF EXISTS (SELECT 1 FROM Exam_Student_Grades WHERE StudentId = @StudentId AND GeneratedExamId = @GeneratedExamId)
                        BEGIN
                            UPDATE Exam_Student_Grades
                            SET GradePercent = @StudentDegreePercent
                            WHERE StudentId = @StudentId AND GeneratedExamId = @GeneratedExamId;
                            SET @result = 0;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO Exam_Student_Grades(StudentId, GeneratedExamId, GradePercent)
                            VALUES(@StudentId, @GeneratedExamId, @StudentDegreePercent);
                            SET @result = SCOPE_IDENTITY();
                        END

                        COMMIT TRANSACTION;
                        RETURN @result;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        RETURN -2;
                    END CATCH
                END');
            END");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('[Exam].[sp_Generate_Exam]'))
BEGIN
Exec('
  CREATE proc [Exam].[sp_Generate_Exam]
  @ExamId int ,
  @DepartmentId int ,
  @BranchId int,
  @NumsTS int ,
  @NumsMCQ int ,
  @CreatedBy int ,
  @TakenDate Date,
  @takenTime Time

 as 
 begin
 Set Nocount On

 declare @GeneratedExamId int ;
 declare @QsOrder int =1;
 declare @TotalGrade int =0;
 declare @ExamName nvarchar(max)='';
 declare @DepartmentBranchId int =0;
 declare @BranchName nvarchar(max)='';
 declare @DepartmentName nvarchar(max)='';


 begin transaction;

 begin try

    --1 get generateExam Name
       select top 1 @DepartmentBranchId = BD.Id ,@DepartmentName=D.Name,@BranchName=B.Name
        from Department_Branches BD join Departments D on BD.DepartmentId=D.Id 
		join Branches B on B.Id=BD.BranchId
        where DepartmentId = @DepartmentId and BranchId = @BranchId;

		PRINT ''@DepartmentBranchId: '' + CAST(@DepartmentBranchId AS NVARCHAR);

        -- Generate ExamName
   SELECT @ExamName = ISNULL(e.Name, ''Unknown Exam'') + '' ( '' + 
                      ISNULL(@DepartmentName, ''Unknown Department'') +'' - ''+
					  ISNULL(@BranchName,''Unknown Branch'')+'' ) ''+
                   '' ('' + CONVERT(NVARCHAR, @TakenDate, 23) + '' '' + 
                       CONVERT(NVARCHAR, @TakenTime, 108) + '')''
           from Exams e

		PRINT ''ExamName: '' + ISNULL(@ExamName, ''NULL'');
		PRINT ''TakenDate: '' + ISNULL(CAST(@TakenDate AS NVARCHAR), ''NULL'');
        PRINT ''TakenTime: '' + ISNULL(CAST(@TakenTime AS NVARCHAR), ''NULL'');

 -- 1 insert a new generated exam 

 insert into  Generated_Exams (DepartmentBranchId,ExamName,TakenDate,TakenTime,CreatedAt,CreatedBy,ExamId,Grade)
 values(
 @DepartmentBranchId,
 @ExamName,
 @TakenDate,
 @takenTime,
 GETDATE(),
 @CreatedBy,
 @ExamId,
 0 -- temp 
 )

 set @GeneratedExamId=SCOPE_IDENTITY();


 -- 2 generate TS qs

 insert into Generated_Exam_Qs (GeneratedExamId,ExamQsId,QsOrder)
 select @GeneratedExamId, Id,  @QsOrder + ROW_NUMBER() over(order by NewID())-1
 from (select top (@NumsTS) Id from Exam_Qs where ExamId=@ExamId and QuestionType=''TF'' order by NewID()) as RandomTS;


 --3 generate MCQ qs
  set @QsOrder=@QsOrder+@NumsTS;

 insert into Generated_Exam_Qs(GeneratedExamId,ExamQsId,QsOrder)
 select @GeneratedExamId ,Id, @QsOrder + ROW_NUMBER() over (order by NewID())-1
 from (select top (@NumsMCQ) Id from Exam_Qs where ExamId=@ExamId and QuestionType=''MCQ'' order by NEWID()) as RandomMCQ;

 --update grade in generated exam

 select @TotalGrade=sum(e.Degree)
 from Generated_Exam_Qs ge
 join Exam_Qs e
 on ge.ExamQsId=e.Id

 update Generated_Exams
 set Grade=@TotalGrade
 where Id=@GeneratedExamId

 commit transaction ;
 return @GeneratedExamId
 end try

 begin catch
  rollback transaction 
  return -1;
  end catch
 end;
')
END;

");


            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Exam.sp_GetStudentAnswers'))
BEGIN
    EXEC('
    CREATE PROC Exam.sp_GetStudentAnswers
        @StudentId INT,
        @GeneratedExamId INT
    AS
    BEGIN
        SET NOCOUNT ON;

        SELECT 
            GEQ.Id AS GeneratedQsId,
            EQ.Question AS Question,
            EQ.QuestionType AS QuestionType,
            GEQ.QsOrder AS QuestionOrder,
            ESA.StdAnswer AS StudentAnswer,
            EQ.Answers AS QuestionAnswers,
            EQ.RightAnswer AS RightAnswer,
            CASE 
                WHEN ESA.StdAnswer = EQ.RightAnswer THEN ''Correct''
                ELSE ''Incorrect''
            END AS AnswerStatus
        FROM Exam_Student_Answers ESA
        JOIN Generated_Exam_Qs GEQ ON ESA.GeneratedExamQsId = GEQ.Id
        JOIN Exam_Qs EQ ON GEQ.ExamQsId = EQ.Id
        WHERE ESA.Generated_Exam_Id = @GeneratedExamId
        AND ESA.StudentId = @StudentId
        ORDER BY GEQ.QsOrder ASC;
    END;
    ')
END;
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetTopics'))
BEGIN
    EXEC('
    CREATE PROC Report.GetTopics
        @CourseId INT
    AS
    BEGIN
        SELECT Name 
        FROM Topics t
        INNER JOIN Course_Topics ct ON ct.TopicId = t.Id
        WHERE ct.CourseId = @CourseId;
    END;
    ')
END;");


            migrationBuilder.Sql(@"

IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetStudentsByDeptID'))
BEGIN
    EXEC('
  CREATE proc [Report].[GetStudentsByDeptID] 
@DeptId int,
@BranchId int 
as
begin
set nocount on
declare  @DepartmentBranchId int =0;
select @DepartmentBranchId=Id from Department_Branches where DepartmentId=@DeptId and BranchId=@BranchId

select * from Students s where s.DepartmentBranchId=@DepartmentBranchId
end
    ')
END;");


            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetQuestionsAndStudentAnswers'))
BEGIN
    EXEC('
    CREATE PROC Report.GetQuestionsAndStudentAnswers
        @GeneratedExamId INT,
        @StudentId INT
    AS
    BEGIN
        SELECT 
            gqs.Id, 
            eqs.Question, 
            eqs.QuestionType, 
            eqs.Answers, 
            eqs.RightAnswer, 
            st.StdAnswer, 
            ge.Grade
        FROM Generated_Exams ge
        INNER JOIN Generated_Exam_Qs gqs ON gqs.GeneratedExamId = ge.Id
        INNER JOIN Exam_Qs eqs ON eqs.Id = gqs.ExamQsId
        INNER JOIN Exam_Student_Answers st ON st.GeneratedExamQsId = gqs.Id
        WHERE st.StudentId = @StudentId AND ge.Id = @GeneratedExamId;
    END;
    ')
END;");


            migrationBuilder.Sql(@"

IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetGrades'))
BEGIN
    EXEC('
    CREATE PROC Report.GetGrades
        @StudentId INT
    AS
    BEGIN
        SET NOCOUNT ON;
        SELECT 
            s.FirstName,
            s.LastName,
            c.Name,
            sc.FinalGradePercent
        FROM Student_Courses sc
        INNER JOIN Students s ON s.Id = sc.StudentId
        INNER JOIN Courses c ON c.Id = sc.CourseId
        WHERE sc.StudentId = @StudentId;
    END;
    ')
END;");

            migrationBuilder.Sql(@"

IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetExamQuestions'))
BEGIN
    EXEC('
    CREATE PROC Report.GetExamQuestions
        @GeneratedExamId INT
    AS
    BEGIN
        SELECT 
            eq.Id, 
            qs.Question, 
            qs.Answers, 
            qs.RightAnswer, 
            qs.QuestionType, 
            qs.Degree, 
            ge.Grade
        FROM Generated_Exams ge
        INNER JOIN Generated_Exam_Qs eq ON ge.Id = eq.GeneratedExamId
        INNER JOIN Exam_Qs qs ON qs.Id = eq.ExamQsId
        WHERE ge.Id = @GeneratedExamId;
    END;
    ')
END;");

            migrationBuilder.Sql(@" 
IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetCoursesAndCapacity'))
BEGIN
    EXEC('
    CREATE PROC Report.GetCoursesAndCapacity
        @InstructorId INT
    AS
    BEGIN
        SET NOCOUNT ON;
        SELECT 
            c.Name, 
            COUNT(s.Id) AS Number_Of_Students
        FROM Instructor_Courses inc
        INNER JOIN Courses c ON c.Id = inc.CourseId
        LEFT JOIN Student_Courses sc ON sc.CourseId = c.Id
        LEFT JOIN Students s ON s.Id = sc.StudentId
        WHERE inc.InstructorId = @InstructorId
        GROUP BY c.Name;
    END;
    ')
END;");






            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllDepartments'))
        BEGIN
            EXEC('
            CREATE PROC GetAllDepartments
            AS
            BEGIN
                SELECT Id, Name FROM Departments;
            END;
            ')
        END;

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetALLStudents'))
        BEGIN
            EXEC('
            CREATE PROC GetALLStudents
            AS
            BEGIN
                SELECT Id, CONCAT(FirstName, '' '', LastName) AS Name FROM Students;
            END;
            ')
        END;

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllInstructors'))
        BEGIN
            EXEC('
            CREATE PROC GetAllInstructors
            AS
            BEGIN
                SELECT Id, CONCAT(FirstName, '' '', LastName) AS Name FROM Instructors;
            END;
            ')
        END;

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllCourses'))
        BEGIN
            EXEC('
            CREATE PROC GetAllCourses
            AS
            BEGIN
                SELECT Id, Name FROM Courses;
            END;
            ')
        END;

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllGeneratedExam'))
        BEGIN
            EXEC('
            CREATE PROC GetAllGeneratedExam
            AS
            BEGIN
                SELECT Id, ExamName FROM Generated_Exams;
            END;
            ')
        END;
        ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Exam.sp_SubmitStudentAnswers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Exam.sp_SubmitSingleStudentAnswer");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Exam.sp_CorrectStudentAnswers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Exam.sp_GetStudentAnswers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Exam.sp_Generate_Exam");


            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Report.GetTopics");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Report.GetStudentsByDeptID");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Report.GetQuestionsAndStudentAnswers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Report.GetGrades");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Report.GetExamQuestions");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Report.GetCoursesAndCapacity");


            migrationBuilder.Sql(@"
        IF EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllDepartments'))
        BEGIN
            DROP PROC GetAllDepartments;
        END;

        IF EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetALLStudents'))
        BEGIN
            DROP PROC GetALLStudents;
        END;

        IF EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllInstructors'))
        BEGIN
            DROP PROC GetAllInstructors;
        END;

        IF EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllCourses'))
        BEGIN
            DROP PROC GetAllCourses;
        END;

        IF EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllGeneratedExam'))
        BEGIN
            DROP PROC GetAllGeneratedExam;
        END;
        ");

        }
    }
}
