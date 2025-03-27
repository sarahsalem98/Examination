using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class ExamDepartment
{
    public int Id { get; set; }

    public int ExamId { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Exam Exam { get; set; } = null!;

    public virtual ICollection<GeneratedExam> GeneratedExams { get; set; } = new List<GeneratedExam>();
}
