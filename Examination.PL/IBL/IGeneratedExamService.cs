using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IGeneratedExamService
    {
        public int GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ, int CreatedBy, DateOnly TakenDate, TimeOnly takenTime);
        public PaginatedData<GeneratedExamMV> GetAllPaginated(int instructor_id,GeneratedExamSearchMV search, int PageSize = 10, int Page = 1);
    }
}
