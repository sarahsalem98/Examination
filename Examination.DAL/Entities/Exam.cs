using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Exam
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public int Duration { get; set; }

    public string? Description { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<ExamDepartment> ExamDepartments { get; set; } = new List<ExamDepartment>();

    public virtual ICollection<ExamQ> ExamQs { get; set; } = new List<ExamQ>();
}
