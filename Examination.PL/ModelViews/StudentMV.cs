using System.ComponentModel.DataAnnotations;

namespace Examination.PL.ModelViews
{
    public class StudentMV
    {
        public int Id { get; set; }

        public int DepartmentBranchId { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [Required]   
        public DateTime? EnrollmentDate { get; set; }
        [Required]
        public int? TrackType { get; set; }

        public int UserId { get; set; }

        public UserMV? User { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int BranchId { get; set; }
        public DepartmentBranchMV? DepartmentBranch { get; set; } 

    }

    public class StudentSearchMV { 
    
        public string? Name { get; set; }
        public int? Status { get; set; } 

        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? TrackType { get; set; }  
        public int? courseId { get; set; }
     }
}
