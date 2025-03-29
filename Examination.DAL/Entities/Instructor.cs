using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Instructor
{
    public int Id { get; set; }
    public int UserId { get; set; } 

    public bool IsExternal { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<GeneratedExam> GeneratedExams { get; set; } = new List<GeneratedExam>();

    public virtual ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();
}
