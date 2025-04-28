using Examination.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Examination.PL.ModelViews
{
    public class ExamMV
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Type { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int CourseId { get; set; }
        public int? Status { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CourseMV? Course { get; set; } = null!;
        public  List<ExamQuestionMV> ExamQs { get; set; } = new List<ExamQuestionMV>();

        public List<GeneratedExamMV> GeneratedExams { get; set; } = new List<GeneratedExamMV>();

    }

    public class ExamSearchMV
    {
        public int? CourseId { get; set; }
        public int? Status { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }

    }

    public class ExamQuestionMV
    {
        public int Id { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public string Question { get; set; } 

        public string Answers { get; set; } = null!;

        public string RightAnswer { get; set; } = null!;

        public int? Level { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Degree must be greater than 0.")]
        [Required(ErrorMessage = "Degree is required.")]
        public int Degree { get; set; }
       

        public string? QuestionType { get; set; }
        public int? Status { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public  ExamMV Exam { get; set; } = null!;


    }

    public class ExamQuestionSearchMV
    {
        public int? CourseId { get; set; }
        public string? Type { get; set; }
        public string? QuestionType { get; set; }
        public int? Status { get; set; }
    }


    public class OnGoingExamMV { 
        public string? ExamTitle { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public List<OnGoingExamQuestion> Questions { get; set; } = new List<OnGoingExamQuestion>();



    }
    public class OnGoingExamQuestion
    {
        public int QId { get; set; }
        public int ? GeneratedExamId { get; set; }
        public int Qorder { get; set; }

        public string? Question { get;set; }
        public string? Answers { get; set; }
        public string ? RightAnswer { get; set; }
        public int Degree { get; set; }

    }

}
