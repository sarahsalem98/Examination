﻿// <auto-generated />
using System;
using Examination.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Examination.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Examination.DAL.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id")
                        .HasName("PK__Branches__3214EC0765A92EC8");

                    b.ToTable("Branches", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Hours")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Courses__3214EC074391AACD");

                    b.ToTable("Courses", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.CourseTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Course_T__3214EC078528DAB5");

                    b.HasIndex("CourseId");

                    b.HasIndex(new[] { "TopicId", "CourseId" }, "UQ__Course_T__6EBCD84682532A19")
                        .IsUnique();

                    b.ToTable("Course_Topics", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Departme__3214EC0748EFB9B1");

                    b.ToTable("Departments", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.DepartmentBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Departme__3214EC078803B4B7");

                    b.HasIndex("BranchId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Department_Branches", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.Exam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Exams__3214EC077FD2C1BE");

                    b.HasIndex("CourseId");

                    b.ToTable("Exams", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamQ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Degree")
                        .HasColumnType("int");

                    b.Property<int>("ExamId")
                        .HasColumnType("int");

                    b.Property<int?>("Level")
                        .HasColumnType("int")
                        .HasColumnName("level");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionType")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("RightAnswer")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Exam_Qs__3214EC07A22B1840");

                    b.HasIndex("ExamId");

                    b.ToTable("Exam_Qs", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamStudentAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GeneratedExamId")
                        .HasColumnType("int")
                        .HasColumnName("Generated_Exam_Id");

                    b.Property<int>("GeneratedExamQsId")
                        .HasColumnType("int");

                    b.Property<string>("StdAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SubmittedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("Id")
                        .HasName("PK__Exam_Stu__3214EC07D872DF03");

                    b.HasIndex("GeneratedExamId");

                    b.HasIndex("GeneratedExamQsId");

                    b.HasIndex("StudentId");

                    b.ToTable("Exam_Student_Answers", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamStudentGrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("GeneratedExamId")
                        .HasColumnType("int");

                    b.Property<decimal>("GradePercent")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Exam_Stu__3214EC0743AE67B2");

                    b.HasIndex("GeneratedExamId");

                    b.HasIndex("StudentId");

                    b.ToTable("Exam_Student_Grades", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.GeneratedExam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentBranchId")
                        .HasColumnType("int");

                    b.Property<int>("ExamId")
                        .HasColumnType("int");

                    b.Property<string>("ExamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<DateOnly>("TakenDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("TakenTime")
                        .HasColumnType("time");

                    b.HasKey("Id")
                        .HasName("PK__Generate__3214EC071AFF4C0C");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DepartmentBranchId");

                    b.HasIndex("ExamId");

                    b.ToTable("Generated_Exams", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.GeneratedExamQ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExamQsId")
                        .HasColumnType("int");

                    b.Property<int>("GeneratedExamId")
                        .HasColumnType("int");

                    b.Property<int>("QsOrder")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Generate__3214EC07D8119518");

                    b.HasIndex("ExamQsId");

                    b.HasIndex("GeneratedExamId");

                    b.ToTable("Generated_Exam_Qs", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.Instructor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsExternal")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Instruct__3214EC07FE3406E8");

                    b.HasIndex(new[] { "UserId" }, "IX_Instructors_UserId");

                    b.ToTable("Instructors", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.InstructorCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int?>("DepartmentBranchId")
                        .HasColumnType("int");

                    b.Property<int>("InstructorId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Instruct__3214EC07AF3B5184");

                    b.HasIndex("CourseId");

                    b.HasIndex("DepartmentBranchId");

                    b.HasIndex(new[] { "InstructorId", "CourseId" }, "UQ__Instruct__F193DD805F5B51C7")
                        .IsUnique();

                    b.ToTable("Instructor_Courses", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<int>("DepartmentBranchId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TrackType")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Students__3214EC07875E9956");

                    b.HasIndex("DepartmentBranchId");

                    b.HasIndex(new[] { "UserId" }, "IX_Students_UserId");

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.StudentCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int?>("FinalGradePercent")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Student___3214EC0752963AE5");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Student_Courses", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Topics__3214EC073196F110");

                    b.ToTable("Topics", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserTypes", (string)null);
                });

            modelBuilder.Entity("UserUserType", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "UserTypeId");

                    b.HasIndex(new[] { "UserTypeId" }, "IX_UserUserTypes_UserTypeId");

                    b.ToTable("UserUserTypes", (string)null);
                });

            modelBuilder.Entity("Examination.DAL.Entities.CourseTopic", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Course", "Course")
                        .WithMany("CourseTopics")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK__Course_To__Cours__0D7A0286");

                    b.HasOne("Examination.DAL.Entities.Topic", "Topic")
                        .WithMany("CourseTopics")
                        .HasForeignKey("TopicId")
                        .IsRequired()
                        .HasConstraintName("FK__Course_To__Topic__0C85DE4D");

                    b.Navigation("Course");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Examination.DAL.Entities.DepartmentBranch", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Branch", "Branch")
                        .WithMany("DepartmentBranches")
                        .HasForeignKey("BranchId")
                        .IsRequired()
                        .HasConstraintName("FK__Departmen__Branc__3A4CA8FD");

                    b.HasOne("Examination.DAL.Entities.Department", "Department")
                        .WithMany("DepartmentBranches")
                        .HasForeignKey("DepartmentId")
                        .IsRequired()
                        .HasConstraintName("FK__Departmen__Depar__395884C4");

                    b.Navigation("Branch");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Exam", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Course", "Course")
                        .WithMany("Exams")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK__Exams__CourseId__160F4887");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamQ", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Exam", "Exam")
                        .WithMany("ExamQs")
                        .HasForeignKey("ExamId")
                        .IsRequired()
                        .HasConstraintName("FK__Exam_Qs__ExamId__10566F31");

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamStudentAnswer", b =>
                {
                    b.HasOne("Examination.DAL.Entities.GeneratedExam", "GeneratedExam")
                        .WithMany("ExamStudentAnswers")
                        .HasForeignKey("GeneratedExamId")
                        .IsRequired()
                        .HasConstraintName("FK__Exam_Stud__Gener__114A936A");

                    b.HasOne("Examination.DAL.Entities.GeneratedExamQ", "GeneratedExamQs")
                        .WithMany("ExamStudentAnswers")
                        .HasForeignKey("GeneratedExamQsId")
                        .IsRequired()
                        .HasConstraintName("FK__Exam_Stud__Gener__123EB7A3");

                    b.HasOne("Examination.DAL.Entities.Student", "Student")
                        .WithMany("ExamStudentAnswers")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__Exam_Stud__Stude__1332DBDC");

                    b.Navigation("GeneratedExam");

                    b.Navigation("GeneratedExamQs");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamStudentGrade", b =>
                {
                    b.HasOne("Examination.DAL.Entities.GeneratedExam", "GeneratedExam")
                        .WithMany("ExamStudentGrades")
                        .HasForeignKey("GeneratedExamId")
                        .IsRequired()
                        .HasConstraintName("FK__Exam_Stud__Gener__14270015");

                    b.HasOne("Examination.DAL.Entities.Student", "Student")
                        .WithMany("ExamStudentGrades")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__Exam_Stud__Stude__151B244E");

                    b.Navigation("GeneratedExam");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Examination.DAL.Entities.GeneratedExam", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Instructor", "CreatedByNavigation")
                        .WithMany("GeneratedExams")
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Generated__Creat__19DFD96B");

                    b.HasOne("Examination.DAL.Entities.DepartmentBranch", "DepartmentBranch")
                        .WithMany("GeneratedExams")
                        .HasForeignKey("DepartmentBranchId")
                        .IsRequired()
                        .HasConstraintName("FK_Generated_Exams_DepartmentBranch");

                    b.HasOne("Examination.DAL.Entities.Exam", "Exam")
                        .WithMany("GeneratedExams")
                        .HasForeignKey("ExamId")
                        .IsRequired()
                        .HasConstraintName("FK_GeneratedExams_Exam");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("DepartmentBranch");

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("Examination.DAL.Entities.GeneratedExamQ", b =>
                {
                    b.HasOne("Examination.DAL.Entities.ExamQ", "ExamQs")
                        .WithMany("GeneratedExamQs")
                        .HasForeignKey("ExamQsId")
                        .IsRequired()
                        .HasConstraintName("FK__Generated__ExamQ__17F790F9");

                    b.HasOne("Examination.DAL.Entities.GeneratedExam", "GeneratedExam")
                        .WithMany("GeneratedExamQs")
                        .HasForeignKey("GeneratedExamId")
                        .IsRequired()
                        .HasConstraintName("FK__Generated__Gener__17036CC0");

                    b.Navigation("ExamQs");

                    b.Navigation("GeneratedExam");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Instructor", b =>
                {
                    b.HasOne("Examination.DAL.Entities.User", "User")
                        .WithMany("Instructors")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Examination.DAL.Entities.InstructorCourse", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Course", "Course")
                        .WithMany("InstructorCourses")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK__Instructo__Cours__1BC821DD");

                    b.HasOne("Examination.DAL.Entities.DepartmentBranch", "DepartmentBranch")
                        .WithMany("InstructorCourses")
                        .HasForeignKey("DepartmentBranchId")
                        .HasConstraintName("FK_InstructorCourses_DepartmentBranch");

                    b.HasOne("Examination.DAL.Entities.Instructor", "Instructor")
                        .WithMany("InstructorCourses")
                        .HasForeignKey("InstructorId")
                        .IsRequired()
                        .HasConstraintName("FK__Instructo__Instr__1AD3FDA4");

                    b.Navigation("Course");

                    b.Navigation("DepartmentBranch");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Student", b =>
                {
                    b.HasOne("Examination.DAL.Entities.DepartmentBranch", "DepartmentBranch")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentBranchId")
                        .IsRequired()
                        .HasConstraintName("Fk_Students_DepartmentBranch");

                    b.HasOne("Examination.DAL.Entities.User", "User")
                        .WithMany("Students")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DepartmentBranch");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Examination.DAL.Entities.StudentCourse", b =>
                {
                    b.HasOne("Examination.DAL.Entities.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK__Student_C__Cours__1F98B2C1");

                    b.HasOne("Examination.DAL.Entities.Student", "Student")
                        .WithMany("StudentCourses")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__Student_C__Stude__1EA48E88");

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("UserUserType", b =>
                {
                    b.HasOne("Examination.DAL.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Examination.DAL.Entities.UserType", null)
                        .WithMany()
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Examination.DAL.Entities.Branch", b =>
                {
                    b.Navigation("DepartmentBranches");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Course", b =>
                {
                    b.Navigation("CourseTopics");

                    b.Navigation("Exams");

                    b.Navigation("InstructorCourses");

                    b.Navigation("StudentCourses");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Department", b =>
                {
                    b.Navigation("DepartmentBranches");
                });

            modelBuilder.Entity("Examination.DAL.Entities.DepartmentBranch", b =>
                {
                    b.Navigation("GeneratedExams");

                    b.Navigation("InstructorCourses");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Exam", b =>
                {
                    b.Navigation("ExamQs");

                    b.Navigation("GeneratedExams");
                });

            modelBuilder.Entity("Examination.DAL.Entities.ExamQ", b =>
                {
                    b.Navigation("GeneratedExamQs");
                });

            modelBuilder.Entity("Examination.DAL.Entities.GeneratedExam", b =>
                {
                    b.Navigation("ExamStudentAnswers");

                    b.Navigation("ExamStudentGrades");

                    b.Navigation("GeneratedExamQs");
                });

            modelBuilder.Entity("Examination.DAL.Entities.GeneratedExamQ", b =>
                {
                    b.Navigation("ExamStudentAnswers");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Instructor", b =>
                {
                    b.Navigation("GeneratedExams");

                    b.Navigation("InstructorCourses");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Student", b =>
                {
                    b.Navigation("ExamStudentAnswers");

                    b.Navigation("ExamStudentGrades");

                    b.Navigation("StudentCourses");
                });

            modelBuilder.Entity("Examination.DAL.Entities.Topic", b =>
                {
                    b.Navigation("CourseTopics");
                });

            modelBuilder.Entity("Examination.DAL.Entities.User", b =>
                {
                    b.Navigation("Instructors");

                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
