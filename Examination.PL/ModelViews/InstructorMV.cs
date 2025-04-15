using Examination.DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Examination.PL.ModelViews
{
    public class InstructorMV
    {
        
        public int ?Id { get; set; }

        [Required(ErrorMessage = "Instructor type (External/Internal) is required.")]
        public bool IsExternal { get; set; }

      
        public int? UserId { get; set; }

        public List<InstructorCourseMV> InstructorCourses { get; set; } = new List<InstructorCourseMV>();
        public UserMV? User { get; set; }
    }
    public class InstructorSearchMV
    {
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? Status { get; set; }
        public bool ? IsExternal { get; set; }

    }

}
