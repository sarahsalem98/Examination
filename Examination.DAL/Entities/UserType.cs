using System;
using System.Collections.Generic;

namespace Examination.DAL.Entities;

public partial class UserType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
