using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Branch
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual ICollection<DepartmentBranch> DepartmentBranches { get; set; } = new List<DepartmentBranch>();
}
