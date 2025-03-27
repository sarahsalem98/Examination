using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class GeneratedExamQ
{
    public int Id { get; set; }

    public int GeneratedExamId { get; set; }

    public int QsOrder { get; set; }

    public int ExamQsId { get; set; }

    public virtual ExamQ ExamQs { get; set; } = null!;

    public virtual ICollection<ExamStudentAnswer> ExamStudentAnswers { get; set; } = new List<ExamStudentAnswer>();

    public virtual GeneratedExam GeneratedExam { get; set; } = null!;
}
