using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IExamService
    {
        public PaginatedData<ExamMV> GetAllPaginated(ExamSearchMV SearchModel, int PageSize = 10, int Page = 1);
        public ExamMV GetById(int id);
        public int ChnageStatus(int id, int status);
        public int Add(ExamMV exam);
        public int Update(ExamMV exam);
    }
}
