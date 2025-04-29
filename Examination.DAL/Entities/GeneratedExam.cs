using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class GeneratedExam
{
    public int Id { get; set; }

    public int DepartmentBranchId { get; set; }

    public string ExamName { get; set; } = null!;

    public DateOnly TakenDate { get; set; }

    public TimeOnly TakenTime { get; set; }

    public int Grade { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int ExamId { get; set; }
    public int? CountTF { get; set; }
    public int? CountMCQ { get; set; }
    public int ? InstructorCourseId { get; set; }

    public virtual Instructor CreatedByNavigation { get; set; } = null!;

    public virtual DepartmentBranch DepartmentBranch { get; set; } = null!;

    public virtual Exam Exam { get; set; } = null!;

    public virtual ICollection<ExamStudentAnswer> ExamStudentAnswers { get; set; } = new List<ExamStudentAnswer>();

    public virtual ICollection<ExamStudentGrade> ExamStudentGrades { get; set; } = new List<ExamStudentGrade>();

    public virtual ICollection<GeneratedExamQ> GeneratedExamQs { get; set; } = new List<GeneratedExamQ>();
    public virtual InstructorCourse? InstructorCourse { get; set; } = null!;    
}
