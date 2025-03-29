using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public int? Age { get; set; }

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }  
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual IList<UserUserType> Types { get; set; } 
    }
}
