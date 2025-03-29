using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DepartmentBranchId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public int? TrackType { get; set; }

    public virtual User User { get; set; }
    public virtual DepartmentBranch DepartmentBranch { get; set; } = null!;

    public virtual ICollection<ExamStudentAnswer> ExamStudentAnswers { get; set; } = new List<ExamStudentAnswer>();

    public virtual ICollection<ExamStudentGrade> ExamStudentGrades { get; set; } = new List<ExamStudentGrade>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
