using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{

    public partial class storedProcedures : Migration
    {
     
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_SubmitStudentAnswers')
                    BEGIN
                        EXEC('create proc Exam.sp_SubmitStudentAnswers    @StudentId int ,    @GeneratedExamId int ,    @StudentAnswers ExamAnswerType readonly    as     begin       set Nocount on        begin TRANSACTION;        begin try            INSERT INTO Exam_Student_Answers (StudentId, GeneratedExamQsId, Generated_Exam_Id, StdAnswer,SubmittedAt)          SELECT @StudentId, GeneratedExamQsId, @GeneratedExamId, StdAnswer,GETDATE()          FROM @StudentAnswers;            COMMIT TRANSACTION;          return 1;      END TRY      begin catch            rollback TRANSACTION;          return -1;      end catch  end');
                    END");

            migrationBuilder.Sql(@"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_SubmitSingleStudentAnswer')
                    BEGIN
  EXEC('CREATE PROC Exam.sp_SubmitSingleStudentAnswer 
        @StudentId INT,
        @GeneratedExamId INT,
        @GeneratedExamQsId INT,
        @StdAnswer NVARCHAR(MAX)
    AS
    BEGIN
        SET NOCOUNT ON;
        BEGIN TRANSACTION;
        BEGIN TRY
            INSERT INTO Exam.Exam_Student_Answers (StudentId, GeneratedExamQsId, Generated_Exam_Id, StdAnswer, SubmittedAt)
            VALUES (@StudentId, @GeneratedExamQsId, @GeneratedExamId, @StdAnswer, GETDATE());

            COMMIT TRANSACTION;
            RETURN 1;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            RETURN -1;
        END CATCH
    END')
                    END");

            migrationBuilder.Sql(@"
                 IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_UpdateSingleStudentAnswer')
                    BEGIN
                    EXEC('  CREATE PROC [Exam].[sp_UpdateSingleStudentAnswer]      @StudentId INT,      @GeneratedExamId INT,      @GeneratedExamQsId INT,      @StdAnswer NVARCHAR(MAX)  AS  BEGIN      SET NOCOUNT ON;      BEGIN TRANSACTION;        BEGIN TRY          -- Check if the record exists          IF NOT EXISTS (              SELECT 1              FROM Exam_Student_Answers              WHERE StudentId = @StudentId                AND Generated_Exam_Id = @GeneratedExamId                AND GeneratedExamQsId = @GeneratedExamQsId          )          BEGIN              -- If no record exists              RETURN -1; -- No row found          END            -- Update the existing answer          UPDATE Exam_Student_Answers          SET StdAnswer = @StdAnswer,              SubmittedAt = GETDATE()          WHERE StudentId = @StudentId            AND Generated_Exam_Id = @GeneratedExamId            AND GeneratedExamQsId = @GeneratedExamQsId;            -- Check how many rows were affected          IF @@ROWCOUNT = 0          BEGIN              -- If no rows were updated              RETURN -1;          END            COMMIT TRANSACTION;          RETURN 1; -- Success      END TRY      BEGIN CATCH          ROLLBACK TRANSACTION;          RETURN -1; -- Error      END CATCH  END      ')
                     
                    END
                  ");

            migrationBuilder.Sql(@"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_CorrectStudentAnswers')
                    BEGIN
EXEC('CREATE PROC [Exam].[sp_CorrectStudentAnswers]
    @StudentId INT,
    @GeneratedExamId INT,
    @InstructorCourseId INT,
    @MinSuccessPrecent INT,
    @ExamType VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalExamGrade INT;
    DECLARE @CorrectTotalDegree INT;
    DECLARE @StudentDegreePercent INT;
    DECLARE @result INT;
    DECLARE @WasAlreadyPassed BIT;

    BEGIN TRANSACTION;
    BEGIN TRY
        SELECT @TotalExamGrade = Grade
        FROM Generated_Exams
        WHERE Id = @GeneratedExamId;

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

        SET @StudentDegreePercent = ISNULL((@CorrectTotalDegree * 100) / @TotalExamGrade, 0);

        SELECT @WasAlreadyPassed =
            CASE
                WHEN EXISTS (
                    SELECT 1 FROM Exam_Student_Grades
                    WHERE StudentId = @StudentId
                      AND GeneratedExamId = @GeneratedExamId
                      AND GradePercent >= @MinSuccessPrecent
                ) THEN 1 ELSE 0
            END;

        IF EXISTS (
            SELECT 1 FROM Exam_Student_Grades
            WHERE StudentId = @StudentId AND GeneratedExamId = @GeneratedExamId
        )
            UPDATE Exam_Student_Grades
            SET GradePercent = @StudentDegreePercent
            WHERE StudentId = @StudentId AND GeneratedExamId = @GeneratedExamId;
        ELSE
            INSERT INTO Exam_Student_Grades(StudentId, GeneratedExamId, GradePercent)
            VALUES(@StudentId, @GeneratedExamId, @StudentDegreePercent);

        IF @ExamType = ''final''
        BEGIN
            IF @StudentDegreePercent >= @MinSuccessPrecent AND @WasAlreadyPassed = 0
            BEGIN
                UPDATE Instructor_Courses
                SET FinalPassedStudentCount = ISNULL(FinalPassedStudentCount, 0) + 1
                WHERE Id = @InstructorCourseId;

                UPDATE Student_Courses
                SET FinalGradePercent = @StudentDegreePercent
                WHERE StudentId = @StudentId AND CourseId = (
                    SELECT C.Id
                    FROM Generated_Exams GE
                    JOIN Exams E ON GE.ExamId = E.Id
                    JOIN Courses C ON E.CourseId = C.Id
                    WHERE GE.Id = @GeneratedExamId
                )
            END
            SET @result =
                CASE WHEN @StudentDegreePercent >= @MinSuccessPrecent THEN 0 ELSE -3 END;
        END
        ELSE IF @ExamType = ''secondtry''
        BEGIN
            IF @StudentDegreePercent >= @MinSuccessPrecent AND @WasAlreadyPassed = 0
            BEGIN
                UPDATE Instructor_Courses
                SET CorrectivePassedStudentCount = ISNULL(CorrectivePassedStudentCount, 0) + 1
                WHERE Id = @InstructorCourseId;
            END

            UPDATE Student_Courses
            SET FinalGradePercent = @StudentDegreePercent
            WHERE StudentId = @StudentId AND CourseId = (
                SELECT C.Id
                FROM Generated_Exams GE
                JOIN Exams E ON GE.ExamId = E.Id
                JOIN Courses C ON E.CourseId = C.Id
                WHERE GE.Id = @GeneratedExamId
            )
            SET @result = 0;
        END
        ELSE
        BEGIN
            SET @result = -4;
        END

        COMMIT TRANSACTION;
        RETURN @result;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RETURN -2;
    END CATCH
END')
                    END");

            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('[Exam].[sp_Generate_Exam]'))
        BEGIN
  EXEC('
    CREATE PROCEDURE [Exam].[sp_Generate_Exam]
        @ExamId INT,
        @DepartmentId INT,
        @BranchId INT,
        @NumsTS INT,
        @NumsMCQ INT,
        @CreatedBy INT,
        @TakenDate DATE,
        @TakenTime TIME
    AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @GeneratedExamId INT;
        DECLARE @QsOrder INT = 1;
        DECLARE @TotalGrade INT = 0;
        DECLARE @ExamName NVARCHAR(MAX) = '''';
        DECLARE @DepartmentBranchId INT = 0;
        DECLARE @BranchName NVARCHAR(MAX) = '''';
        DECLARE @DepartmentName NVARCHAR(MAX) = '''';
        DECLARE @CourseId INT;
        DECLARE @InstructorCourseId INT;

        BEGIN TRANSACTION;

        BEGIN TRY
            SELECT TOP 1 
                @DepartmentBranchId = BD.Id,
                @DepartmentName = D.Name,
                @BranchName = B.Name
            FROM Department_Branches BD
            JOIN Departments D ON BD.DepartmentId = D.Id
            JOIN Branches B ON B.Id = BD.BranchId
            WHERE BD.DepartmentId = @DepartmentId AND BD.BranchId = @BranchId;

            SELECT @CourseId = CourseId FROM Exams WHERE Id = @ExamId;

            SELECT @InstructorCourseId = Id
            FROM Instructor_Courses
            WHERE CourseId = @CourseId
              AND InstructorId = @CreatedBy
              AND DepartmentBranchId = @DepartmentBranchId
              AND YEAR(EndDate) = YEAR(GETDATE());

            SELECT @ExamName = ISNULL(e.Name, ''Unknown Exam'') + '' ('' +
                                  ISNULL(@DepartmentName, ''Unknown Department'') + '' - '' +
                                  ISNULL(@BranchName, ''Unknown Branch'') + '') '' +
                                  ''('' + CONVERT(NVARCHAR, @TakenDate, 23) + '' '' +
                                  CONVERT(NVARCHAR, @TakenTime, 108) + '')''
            FROM Exams e WHERE e.Id = @ExamId;

            INSERT INTO Generated_Exams (
                DepartmentBranchId, ExamName, TakenDate, TakenTime, 
                CreatedAt, CreatedBy, ExamId, Grade, CountMCQ, CountTF, InstructorCourseId
            )
            VALUES (
                @DepartmentBranchId, @ExamName, @TakenDate, @TakenTime,
                GETDATE(), @CreatedBy, @ExamId, 0, @NumsMCQ, @NumsTS, @InstructorCourseId
            );

            SET @GeneratedExamId = SCOPE_IDENTITY();

            INSERT INTO Generated_Exam_Qs (GeneratedExamId, ExamQsId, QsOrder)
            SELECT @GeneratedExamId, Id, @QsOrder + ROW_NUMBER() OVER (ORDER BY NEWID()) - 1
            FROM (
                SELECT TOP (@NumsTS) Id FROM Exam_Qs 
                WHERE ExamId = @ExamId AND QuestionType = ''TF'' 
                ORDER BY NEWID()
            ) AS RandomTS;

            SET @QsOrder = @QsOrder + @NumsTS;

            INSERT INTO Generated_Exam_Qs (GeneratedExamId, ExamQsId, QsOrder)
            SELECT @GeneratedExamId, Id, @QsOrder + ROW_NUMBER() OVER (ORDER BY NEWID()) - 1
            FROM (
                SELECT TOP (@NumsMCQ) Id FROM Exam_Qs 
                WHERE ExamId = @ExamId AND QuestionType = ''MCQ'' 
                ORDER BY NEWID()
            ) AS RandomMCQ;

            SELECT @TotalGrade = SUM(e.Degree)
            FROM Generated_Exam_Qs ge
            JOIN Exam_Qs e ON ge.ExamQsId = e.Id
            WHERE ge.GeneratedExamId = @GeneratedExamId;

            UPDATE Generated_Exams
            SET Grade = @TotalGrade
            WHERE Id = @GeneratedExamId;

            COMMIT TRANSACTION;
            RETURN @GeneratedExamId;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            RETURN -1;
        END CATCH
    END
    ')
        END;

        ");


            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Exam.sp_GetStudentAnswers'))
        BEGIN
    EXEC('
        CREATE PROCEDURE Exam.sp_GetStudentAnswers
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
            FROM
                Exam_Student_Answers ESA
                JOIN Generated_Exam_Qs GEQ ON ESA.GeneratedExamQsId = GEQ.Id
                JOIN Exam_Qs EQ ON GEQ.ExamQsId = EQ.Id
            WHERE
                ESA.Generated_Exam_Id = @GeneratedExamId
                AND ESA.StudentId = @StudentId
            ORDER BY
                GEQ.QsOrder ASC;
        END
    ')        END;
        ");

            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetTopics'))
        BEGIN
            EXEC('create proc Report.GetTopics  @CourseId int  as  begin  select name from Topics t inner join Course_Topics ct on ct.TopicId=t.Id where ct.CourseId=@CourseId  end')
        END;");


            migrationBuilder.Sql(@"

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetStudentsByDeptID'))
        BEGIN
            EXEC('CREATE proc [Report].[GetStudentsByDeptID]   @DeptId int,  @BranchId int   as  begin  set nocount on  declare  @DepartmentBranchId int =0;  select @DepartmentBranchId=Id from Department_Branches where DepartmentId=@DeptId and BranchId=@BranchId    select u.FirstName, u.LastName,u.Age,u.Email,u.Phone,u.Status,s.EnrollmentDate,s.TrackType,s.DateOfBirth from Students s   join Users u on s.UserId=u.Id   where s.DepartmentBranchId=@DepartmentBranchId  end
            ')
        END;");


            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetQuestionsAndStudentAnswers'))
        BEGIN
            EXEC('create proc Report.GetQuestionsAndStudentAnswers  @GeneratedExamId int,  @StudentId int  as  begin  select gqs.Id, eqs.Question,eqs.QuestionType,eqs.Answers,eqs.RightAnswer,st.StdAnswer,ge.Grade  from Generated_Exams ge   inner join Generated_Exam_Qs gqs on gqs.GeneratedExamId=ge.Id  inner join Exam_Qs eqs on eqs.Id=gqs.ExamQsId  inner join Exam_Student_Answers st on st.GeneratedExamQsId=gqs.Id  where st.StudentId=@StudentId and ge.Id=@GeneratedExamId  end')
        END;");


            migrationBuilder.Sql(@"

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetGrades'))
        BEGIN
            EXEC('CREATE proc [Report].[GetGrades]   @StudentId int  as  begin  set nocount on  select U.FirstName,U.LastName,c.Name,sc.FinalGradePercent   from Student_Courses sc inner join Students s on s.Id=sc.StudentId inner join Courses c on c.Id=sc.CourseId  inner join Users U on U.Id=s.UserId  where  sc.StudentId=@StudentId  end')
            END;");

            migrationBuilder.Sql(@"

        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetExamQuestions'))
        BEGIN
            EXEC('  CREATE proc Report.GetExamQuestions   @GeneratedExamId int  as  begin  select eq.Id, qs.Question,qs.Answers,qs.RightAnswer,qs.QuestionType,qs.Degree,ge.Grade  from Generated_Exams ge   inner join Generated_Exam_Qs eq on ge.Id=eq.GeneratedExamId  inner join Exam_Qs qs on qs.Id=eq.ExamQsId    where ge.Id=@GeneratedExamId  end')
        END;");

            migrationBuilder.Sql(@" 
        IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('Report.GetCoursesAndCapacity'))
        BEGIN
            EXEC('CREATE proc [Report].[GetCoursesAndCapacity]  @InstructorId int  as  begin    select c.Name,count(s.Id) as Number_Of_Students from Instructor_Courses inc   inner join Courses c on c.Id=inc.CourseId  left join Student_Courses sc on sc.CourseId=c.Id  left join Students s on s.Id=sc.StudentId  where inc.InstructorId=@InstructorId  group by c.Name  end')
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
            SELECT S.Id, CONCAT(U.FirstName, '' '', U.LastName) AS Name
            FROM Students S
            JOIN Users U ON U.Id = S.UserId
        END
    ')
                END;

                IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('GetAllInstructors'))
                BEGIN
                    EXEC('
                    CREATE PROC GetAllInstructors
                    AS
                    BEGIN
                        select I.id,concat(U.firstname,'' '',U.lastname) as Name from Instructors I
                          join Users U on I.UserId=U.Id
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


            migrationBuilder.Sql(@"
           IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE OBJECT_ID = OBJECT_ID('dbo.GetAllBranches'))
        BEGIN
            EXEC('            
        create proc GetAllBranches
        as
        begin
        select id,name from Branches
        end
        ')
        END;");

        }

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


            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetAllBranches");

        }
    }
}
