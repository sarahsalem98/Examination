using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class InstructorCourse
{
    public int Id { get; set; }

    public int InstructorId { get; set; }

    public int CourseId { get; set; }

    public int? DepartmentBranchId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual DepartmentBranch? DepartmentBranch { get; set; }

    public virtual Instructor Instructor { get; set; } = null!;
}
