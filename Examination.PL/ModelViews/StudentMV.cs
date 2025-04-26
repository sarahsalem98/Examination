using Examination.DAL.Entities;
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
        public List<StudentCourseMV>? StudentCourses { get; set; } 


    }

    public class StudentSearchMV { 
    
        public string? Name { get; set; }
        public int? Status { get; set; } 

        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? TrackType { get; set; }  
        public int? courseId { get; set; }
     }
    public class StudentCourseMV
    {
        public int? Id { get; set; }

        public int? StudentId { get; set; }

        public int? CourseId { get; set; }

        public int? FinalGradePercent { get; set; }

        public CourseMV?  Course { get; set; }

        public StudentMV? Student { get; set; }
    }

    public class StudentUpdateProfileMV
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public UserUpdateMV User { get; set; }
    }

    public class UserUpdateMV
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
