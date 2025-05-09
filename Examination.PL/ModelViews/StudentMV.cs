﻿using Examination.DAL.Entities;
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

        public List<ExamStudentAnswerMV> ExamStudentAnswers { get; set; } = new List<ExamStudentAnswerMV>();
        public List<ExamStudentGradeMV> ExamStudentGrades { get; set; } = new List<ExamStudentGradeMV>();
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
    public class ExamStudentAnswerMV
    {
        public int Id { get; set; }

        public int GeneratedExamId { get; set; }

        public int GeneratedExamQsId { get; set; }

        public int StudentId { get; set; }

        public string StdAnswer { get; set; } = null!;

        public DateTime? SubmittedAt { get; set; }

        public GeneratedExamMV GeneratedExam { get; set; } = null!;

        public GeneratedExamQMV GeneratedExamQs { get; set; } = null!;

        public StudentMV Student { get; set; } = null!;

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
    public class ExamStudentGradeMV
    {
        public int Id { get; set; }

        public int GeneratedExamId { get; set; }

        public int StudentId { get; set; }

        public decimal GradePercent { get; set; }

        public DateTime? CreatedAt { get; set; }

        public  GeneratedExamMV GeneratedExam { get; set; } = null!;

        public  StudentMV Student { get; set; } = null!;
    }
   
}
