using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class ExamStudentAnswer
{
    public int Id { get; set; }

    public int GeneratedExamId { get; set; }

    public int GeneratedExamQsId { get; set; }

    public int StudentId { get; set; }

    public string StdAnswer { get; set; } = null!;

    public DateTime? SubmittedAt { get; set; }

    public virtual GeneratedExam GeneratedExam { get; set; } = null!;

    public virtual GeneratedExamQ GeneratedExamQs { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
