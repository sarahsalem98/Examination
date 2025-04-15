using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class ExamQ
{
    public int Id { get; set; }

    public int ExamId { get; set; }

    public string Question { get; set; } = null!;

    public string Answers { get; set; } = null!;

    public string RightAnswer { get; set; } = null!;

    public int? Level { get; set; }

    public int Degree { get; set; }

    public string? QuestionType { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual ICollection<GeneratedExamQ> GeneratedExamQs { get; set; } = new List<GeneratedExamQ>();
}
