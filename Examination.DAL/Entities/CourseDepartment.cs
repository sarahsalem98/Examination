using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Entities
{
   public class CourseDepartment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int DepartmentId { get; set; }
        public virtual Course Course { get; set; } = null!;
        public virtual Department Department { get; set; } = null!;
    }
}
