using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class CourseTopic
{
    public int Id { get; set; }

    public int TopicId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Topic Topic { get; set; } = null!;
}
