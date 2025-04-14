using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IExamService
    {
        public PaginatedData<ExamMV> GetAllPaginated(ExamSearchMV SearchModel, int PageSize = 10, int Page = 1);
    }
}
