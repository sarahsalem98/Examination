using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class InstructorCourse
{
    public int Id { get; set; }

    public int InstructorId { get; set; }

    public int CourseId { get; set; }

    public int? DepartmentBranchId { get; set; }

    public int? TotalStudents { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? LastGeneratedExamType { get; set; } 

    public int? FinalPassedStudentCount { get; set; }
    public int? CorrectivePassedStudentCount { get; set; }

    public int? IsCompleted { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual DepartmentBranch? DepartmentBranch { get; set; }

    public virtual Instructor Instructor { get; set; } = null!;
    public virtual ICollection<GeneratedExam> GeneratedExams { get; set; } = new List<GeneratedExam>();
}
