using Examination.DAL.Entities;
using System.ComponentModel.DataAnnotations;

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

        public InstructorMV CreatedByNavigation { get; set; } = null!;

        public DepartmentBranchMV DepartmentBranch { get; set; } = null!;

        public ExamMV Exam { get; set; } = null!;


    }
    public class GeneratedExamQMV
    {
        public int Id { get; set; }

        public int GeneratedExamId { get; set; }

        public int QsOrder { get; set; }

        public int ExamQsId { get; set; }

        public virtual ExamQ ExamQs { get; set; } = null!;

        public  List<ExamStudentAnswerMV> ExamStudentAnswers { get; set; } = new List<ExamStudentAnswerMV>();

        public GeneratedExamMV GeneratedExam { get; set; } = null!;
    }
    public class ExamStudentAnswerMV
        {
        public int Id { get; set; }

        public int GeneratedExamId { get; set; }

        public int GeneratedExamQsId { get; set; }

        public int StudentId { get; set; }

        public string StdAnswer { get; set; } = null!;

        public DateTime? SubmittedAt { get; set; }

        public  GeneratedExamMV GeneratedExam { get; set; } = null!;

        public  GeneratedExamQMV GeneratedExamQs { get; set; } = null!;

        public  StudentMV Student { get; set; } = null!;

    }
    public class GeneratedExamSearchMV
    {
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public int? BranchId { get;set; }
        public int? DepartmentId { get; set; }
        public string ?ExamType { get; set; }
    }
}
