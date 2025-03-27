using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class StudentCourse
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int? FinalGradePercent { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
