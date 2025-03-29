using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initialMigrationAfterDbScaffold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Branches__3214EC0765A92EC8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Courses__3214EC074391AACD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departme__3214EC0748EFB9B1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Topics__3214EC073196F110", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exams__3214EC077FD2C1BE", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exams__CourseId__160F4887",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Department_Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departme__3214EC078803B4B7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Departmen__Branc__3A4CA8FD",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Departmen__Depar__395884C4",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Course_Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course_T__3214EC078528DAB5", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Course_To__Cours__0D7A0286",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Course_To__Topic__0C85DE4D",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Instruct__3214EC07FE3406E8", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserUserTypes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserTypes", x => new { x.UserId, x.UserTypeId });
                    table.ForeignKey(
                        name: "FK_UserUserTypes_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUserTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exam_Qs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightAnswer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    level = table.Column<int>(type: "int", nullable: true),
                    Degree = table.Column<int>(type: "int", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exam_Qs__3214EC07A22B1840", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exam_Qs__ExamId__10566F31",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentBranchId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrackType = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students__3214EC07875E9956", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Fk_Students_DepartmentBranch",
                        column: x => x.DepartmentBranchId,
                        principalTable: "Department_Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Generated_Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentBranchId = table.Column<int>(type: "int", nullable: false),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TakenDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TakenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Generate__3214EC071AFF4C0C", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedExams_Exam",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Generated_Exams_DepartmentBranch",
                        column: x => x.DepartmentBranchId,
                        principalTable: "Department_Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Generated__Creat__19DFD96B",
                        column: x => x.CreatedBy,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Instructor_Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    DepartmentBranchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Instruct__3214EC07AF3B5184", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorCourses_DepartmentBranch",
                        column: x => x.DepartmentBranchId,
                        principalTable: "Department_Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Instructo__Cours__1BC821DD",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Instructo__Instr__1AD3FDA4",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Student_Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    FinalGradePercent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student___3214EC0752963AE5", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Student_C__Cours__1F98B2C1",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Student_C__Stude__1EA48E88",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exam_Student_Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneratedExamId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GradePercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exam_Stu__3214EC0743AE67B2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exam_Stud__Gener__14270015",
                        column: x => x.GeneratedExamId,
                        principalTable: "Generated_Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Exam_Stud__Stude__151B244E",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Generated_Exam_Qs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneratedExamId = table.Column<int>(type: "int", nullable: false),
                    QsOrder = table.Column<int>(type: "int", nullable: false),
                    ExamQsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Generate__3214EC07D8119518", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Generated__ExamQ__17F790F9",
                        column: x => x.ExamQsId,
                        principalTable: "Exam_Qs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Generated__Gener__17036CC0",
                        column: x => x.GeneratedExamId,
                        principalTable: "Generated_Exams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exam_Student_Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Generated_Exam_Id = table.Column<int>(type: "int", nullable: false),
                    GeneratedExamQsId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StdAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exam_Stu__3214EC07D872DF03", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exam_Stud__Gener__114A936A",
                        column: x => x.Generated_Exam_Id,
                        principalTable: "Generated_Exams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Exam_Stud__Gener__123EB7A3",
                        column: x => x.GeneratedExamQsId,
                        principalTable: "Generated_Exam_Qs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Exam_Stud__Stude__1332DBDC",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_Topics_CourseId",
                table: "Course_Topics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "UQ__Course_T__6EBCD84682532A19",
                table: "Course_Topics",
                columns: new[] { "TopicId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_Branches_BranchId",
                table: "Department_Branches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_Branches_DepartmentId",
                table: "Department_Branches",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Qs_ExamId",
                table: "Exam_Qs",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Answers_Generated_Exam_Id",
                table: "Exam_Student_Answers",
                column: "Generated_Exam_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Answers_GeneratedExamQsId",
                table: "Exam_Student_Answers",
                column: "GeneratedExamQsId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Answers_StudentId",
                table: "Exam_Student_Answers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Grades_GeneratedExamId",
                table: "Exam_Student_Grades",
                column: "GeneratedExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Grades_StudentId",
                table: "Exam_Student_Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Generated_Exam_Qs_ExamQsId",
                table: "Generated_Exam_Qs",
                column: "ExamQsId");

            migrationBuilder.CreateIndex(
                name: "IX_Generated_Exam_Qs_GeneratedExamId",
                table: "Generated_Exam_Qs",
                column: "GeneratedExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Generated_Exams_CreatedBy",
                table: "Generated_Exams",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Generated_Exams_DepartmentBranchId",
                table: "Generated_Exams",
                column: "DepartmentBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Generated_Exams_ExamId",
                table: "Generated_Exams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_Courses_CourseId",
                table: "Instructor_Courses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_Courses_DepartmentBranchId",
                table: "Instructor_Courses",
                column: "DepartmentBranchId");

            migrationBuilder.CreateIndex(
                name: "UQ__Instruct__F193DD805F5B51C7",
                table: "Instructor_Courses",
                columns: new[] { "InstructorId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Courses_CourseId",
                table: "Student_Courses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Courses_StudentId",
                table: "Student_Courses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentBranchId",
                table: "Students",
                column: "DepartmentBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserTypes_UserTypeId",
                table: "UserUserTypes",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Course_Topics");

            migrationBuilder.DropTable(
                name: "Exam_Student_Answers");

            migrationBuilder.DropTable(
                name: "Exam_Student_Grades");

            migrationBuilder.DropTable(
                name: "Instructor_Courses");

            migrationBuilder.DropTable(
                name: "Student_Courses");

            migrationBuilder.DropTable(
                name: "UserUserTypes");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Generated_Exam_Qs");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "Exam_Qs");

            migrationBuilder.DropTable(
                name: "Generated_Exams");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Department_Branches");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
