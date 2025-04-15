using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Topic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CourseTopic> CourseTopics { get; set; } = new List<CourseTopic>();
}
