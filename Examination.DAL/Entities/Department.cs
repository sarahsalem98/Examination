﻿using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<DepartmentBranch> DepartmentBranches { get; set; } = new List<DepartmentBranch>();
}
