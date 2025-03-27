using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class ExamStudentGrade
{
    public int Id { get; set; }

    public int GeneratedExamId { get; set; }

    public int StudentId { get; set; }

    public decimal GradePercent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual GeneratedExam GeneratedExam { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
