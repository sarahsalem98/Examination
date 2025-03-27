using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Instructor
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<GeneratedExam> GeneratedExams { get; set; } = new List<GeneratedExam>();

    public virtual ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();

    public virtual ICollection<InstructorDepartment> InstructorDepartments { get; set; } = new List<InstructorDepartment>();
}
