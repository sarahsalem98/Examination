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
        public int? Status {  get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }

    }
}
