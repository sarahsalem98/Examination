using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Examination.DAL.Entities;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseTopic> CourseTopics { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamDepartment> ExamDepartments { get; set; }

    public virtual DbSet<ExamQ> ExamQs { get; set; }

    public virtual DbSet<ExamStudentAnswer> ExamStudentAnswers { get; set; }

    public virtual DbSet<ExamStudentGrade> ExamStudentGrades { get; set; }

    public virtual DbSet<GeneratedExam> GeneratedExams { get; set; }

    public virtual DbSet<GeneratedExamQ> GeneratedExamQs { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<InstructorCourse> InstructorCourses { get; set; }

    public virtual DbSet<InstructorDepartment> InstructorDepartments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=sarahsalem.database.windows.net;Database=Exam;User Id=sarahadmin;Password=Admin?123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC074391AACD");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CourseTopic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course_T__3214EC078528DAB5");

            entity.ToTable("Course_Topics");

            entity.HasIndex(e => new { e.TopicId, e.CourseId }, "UQ__Course_T__6EBCD84682532A19").IsUnique();

            entity.HasOne(d => d.Course).WithMany(p => p.CourseTopics)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Course_To__Cours__0D7A0286");

            entity.HasOne(d => d.Topic).WithMany(p => p.CourseTopics)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Course_To__Topic__0C85DE4D");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC0748EFB9B1");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3214EC077FD2C1BE");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exams__CourseId__160F4887");
        });

        modelBuilder.Entity<ExamDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam_Dep__3214EC07547C6E33");

            entity.ToTable("Exam_Department");

            entity.HasOne(d => d.Department).WithMany(p => p.ExamDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Depa__Depar__0F624AF8");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamDepartments)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Depa__ExamI__0E6E26BF");
        });

        modelBuilder.Entity<ExamQ>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam_Qs__3214EC07A22B1840");

            entity.ToTable("Exam_Qs");

            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.QuestionType).HasMaxLength(10);
            entity.Property(e => e.RightAnswer).HasMaxLength(255);

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQs)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Qs__ExamId__10566F31");
        });

        modelBuilder.Entity<ExamStudentAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam_Stu__3214EC07D872DF03");

            entity.ToTable("Exam_Student_Answers");

            entity.Property(e => e.GeneratedExamId).HasColumnName("Generated_Exam_Id");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.GeneratedExam).WithMany(p => p.ExamStudentAnswers)
                .HasForeignKey(d => d.GeneratedExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Stud__Gener__114A936A");

            entity.HasOne(d => d.GeneratedExamQs).WithMany(p => p.ExamStudentAnswers)
                .HasForeignKey(d => d.GeneratedExamQsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Stud__Gener__123EB7A3");

            entity.HasOne(d => d.Student).WithMany(p => p.ExamStudentAnswers)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Stud__Stude__1332DBDC");
        });

        modelBuilder.Entity<ExamStudentGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam_Stu__3214EC0743AE67B2");

            entity.ToTable("Exam_Student_Grades");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GradePercent).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.GeneratedExam).WithMany(p => p.ExamStudentGrades)
                .HasForeignKey(d => d.GeneratedExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Stud__Gener__14270015");

            entity.HasOne(d => d.Student).WithMany(p => p.ExamStudentGrades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam_Stud__Stude__151B244E");
        });

        modelBuilder.Entity<GeneratedExam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Generate__3214EC071AFF4C0C");

            entity.ToTable("Generated_Exams");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GeneratedExams)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Generated__Creat__19DFD96B");

            entity.HasOne(d => d.ExamDepartment).WithMany(p => p.GeneratedExams)
                .HasForeignKey(d => d.ExamDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Generated__ExamD__18EBB532");
        });

        modelBuilder.Entity<GeneratedExamQ>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Generate__3214EC07D8119518");

            entity.ToTable("Generated_Exam_Qs");

            entity.HasOne(d => d.ExamQs).WithMany(p => p.GeneratedExamQs)
                .HasForeignKey(d => d.ExamQsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Generated__ExamQ__17F790F9");

            entity.HasOne(d => d.GeneratedExam).WithMany(p => p.GeneratedExamQs)
                .HasForeignKey(d => d.GeneratedExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Generated__Gener__17036CC0");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC07FE3406E8");

            entity.HasIndex(e => e.Email, "UQ__Instruct__A9D10534BCA0D73C").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<InstructorCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC07AF3B5184");

            entity.ToTable("Instructor_Courses");

            entity.HasIndex(e => new { e.InstructorId, e.CourseId }, "UQ__Instruct__F193DD805F5B51C7").IsUnique();

            entity.HasOne(d => d.Course).WithMany(p => p.InstructorCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__Cours__1BC821DD");

            entity.HasOne(d => d.Instructor).WithMany(p => p.InstructorCourses)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__Instr__1AD3FDA4");
        });

        modelBuilder.Entity<InstructorDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC073FE72C68");

            entity.ToTable("Instructor_Department");

            entity.HasIndex(e => new { e.InstructorId, e.DepartmentId }, "UQ__Instruct__56217324A10C4FBE").IsUnique();

            entity.HasOne(d => d.Department).WithMany(p => p.InstructorDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__Depar__1DB06A4F");

            entity.HasOne(d => d.Instructor).WithMany(p => p.InstructorDepartments)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__Instr__1CBC4616");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07875E9956");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D105341C9519F1").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EnrollmentDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Department).WithMany(p => p.Students)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Depart__208CD6FA");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student___3214EC0752963AE5");

            entity.ToTable("Student_Courses");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student_C__Cours__1F98B2C1");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student_C__Stude__1EA48E88");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Topics__3214EC073196F110");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
