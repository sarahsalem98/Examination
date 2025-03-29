using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class DepartmentBranch
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public int DepartmentId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<GeneratedExam> GeneratedExams { get; set; } = new List<GeneratedExam>();

    public virtual ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
