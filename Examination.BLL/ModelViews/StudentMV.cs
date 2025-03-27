using Examination.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.BLL.ModelViews
{
   public class StudentMV
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public int? Status { get; set; }
    }

}
