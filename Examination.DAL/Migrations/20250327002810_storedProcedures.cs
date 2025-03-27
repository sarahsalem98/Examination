using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class storedProcedures : Migration
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
    EXEC('
    CREATE PROC [Exam].[sp_Generate_Exam]
        @ExamId INT,
        @DepartmentId INT,
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
        DECLARE @ExamDepartmentId INT = 0;

        BEGIN TRANSACTION;

        BEGIN TRY
            -- Get ExamDepartmentId
            SELECT @ExamDepartmentId = Id
            FROM Exam_Department
            WHERE DepartmentId = @DepartmentId AND ExamId = @ExamId;

            PRINT ''ExamDepartmentId: '' + CAST(@ExamDepartmentId AS NVARCHAR);

            -- Generate ExamName
            SELECT @ExamName = ISNULL(e.Name, ''Unknown Exam'') + '' - '' +
                               ISNULL(d.Name, ''Unknown Department'') +
                               '' ('' + CONVERT(NVARCHAR, @TakenDate, 23) + '' '' +
                               CONVERT(NVARCHAR, @TakenTime, 108) + '')''
            FROM Exam_Department ed
            JOIN Exams e ON ed.ExamId = e.Id
            JOIN Departments d ON ed.DepartmentId = d.Id
            WHERE ed.Id = @ExamDepartmentId;

            PRINT ''ExamName: '' + ISNULL(@ExamName, ''NULL'');
            PRINT ''TakenDate: '' + ISNULL(CAST(@TakenDate AS NVARCHAR), ''NULL'');
            PRINT ''TakenTime: '' + ISNULL(CAST(@TakenTime AS NVARCHAR), ''NULL'');

            -- Insert a new generated exam
            INSERT INTO Generated_Exams (ExamDepartmentId, ExamName, TakenDate, TakenTime, CreatedAt, CreatedBy, Grade)
            VALUES (@ExamDepartmentId, @ExamName, @TakenDate, @TakenTime, GETDATE(), @CreatedBy, 0);

            SET @GeneratedExamId = SCOPE_IDENTITY();

            -- Generate TS questions
            INSERT INTO Generated_Exam_Qs (GeneratedExamId, ExamQsId, QsOrder)
            SELECT @GeneratedExamId, Id, @QsOrder + ROW_NUMBER() OVER (ORDER BY NEWID()) - 1
            FROM (SELECT TOP (@NumsTS) Id FROM Exam_Qs WHERE ExamId = @ExamId AND QuestionType = ''TF'' ORDER BY NEWID()) AS RandomTS;

            -- Generate MCQ questions
            SET @QsOrder = @QsOrder + @NumsTS;

            INSERT INTO Generated_Exam_Qs (GeneratedExamId, ExamQsId, QsOrder)
            SELECT @GeneratedExamId, Id, @QsOrder + ROW_NUMBER() OVER (ORDER BY NEWID()) - 1
            FROM (SELECT TOP (@NumsMCQ) Id FROM Exam_Qs WHERE ExamId = @ExamId AND QuestionType = ''MCQ'' ORDER BY NEWID()) AS RandomMCQ;

            -- Update grade in generated exam
            SELECT @TotalGrade = SUM(e.Degree)
            FROM Generated_Exam_Qs ge
            JOIN Exam_Qs e ON ge.ExamQsId = e.Id;

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
    END;
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
    CREATE PROC Report.GetStudentsByDeptID
        @DeptId INT
    AS
    BEGIN
        SET NOCOUNT ON;
        SELECT * FROM Students s WHERE s.DepartmentId = @DeptId;
    END;
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
