using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<ExamDepartment> ExamDepartments { get; set; } = new List<ExamDepartment>();

    public virtual ICollection<InstructorDepartment> InstructorDepartments { get; set; } = new List<InstructorDepartment>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
