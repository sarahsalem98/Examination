using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class GeneratedExam
{
    public int Id { get; set; }

    public int ExamDepartmentId { get; set; }

    public string ExamName { get; set; } = null!;

    public DateOnly TakenDate { get; set; }

    public TimeOnly TakenTime { get; set; }

    public int Grade { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public virtual Instructor CreatedByNavigation { get; set; } = null!;

    public virtual ExamDepartment ExamDepartment { get; set; } = null!;

    public virtual ICollection<ExamStudentAnswer> ExamStudentAnswers { get; set; } = new List<ExamStudentAnswer>();

    public virtual ICollection<ExamStudentGrade> ExamStudentGrades { get; set; } = new List<ExamStudentGrade>();

    public virtual ICollection<GeneratedExamQ> GeneratedExamQs { get; set; } = new List<GeneratedExamQ>();
}
