using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Hours { get; set; }

    public virtual ICollection<CourseTopic> CourseTopics { get; set; } = new List<CourseTopic>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
