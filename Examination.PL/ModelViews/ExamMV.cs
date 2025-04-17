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

        public int ExamId { get; set; }

        public string Question { get; set; } = null!;

        public string Answers { get; set; } = null!;

        public string RightAnswer { get; set; } = null!;

        public int? Level { get; set; }

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
}
