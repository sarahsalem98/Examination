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

        public int DepartmentBranchId { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public int? TrackType { get; set; }

        public int UserId { get; set; }

        public UserMV? UserMV { get; set; }


    }

}
