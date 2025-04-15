using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class Branch
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }


    public virtual ICollection<DepartmentBranch> DepartmentBranches { get; set; } = new List<DepartmentBranch>();
}
