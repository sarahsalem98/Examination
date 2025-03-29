using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Entities
{
   public class UserUserType
    {
        public int UserId { get; set; }
        public int TypeId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual UserType UserType { get; set; } = null!;

    }
}
