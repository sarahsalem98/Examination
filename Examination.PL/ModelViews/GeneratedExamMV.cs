using Examination.DAL.Entities;

namespace Examination.PL.ModelViews
{
    public class GeneratedExamMV
    {
        public int Id { get; set; }

        public int DepartmentBranchId { get; set; }

        public string ExamName { get; set; } = null!;

        public DateOnly TakenDate { get; set; }

        public TimeOnly TakenTime { get; set; }

        public int Grade { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public int ExamId { get; set; }

        public  InstructorMV CreatedByNavigation { get; set; } = null!;

        public DepartmentBranchMV DepartmentBranch { get; set; } = null!;

        public ExamMV Exam { get; set; } = null!;

     
    }
}
