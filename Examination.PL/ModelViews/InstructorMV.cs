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
    public class InstructorPasswordUpdateMV
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }


}
