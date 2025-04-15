using Examination.DAL.Entities;

namespace Examination.PL.ModelViews
{
    public class ExamMV
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Type { get; set; }
        public int Duration { get; set; }

        public string? Description { get; set; }

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
