using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class InstructorDepartment
{
    public int Id { get; set; }

    public int InstructorId { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Instructor Instructor { get; set; } = null!;
}
