using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IInstructorService
    {
        public PaginatedData<InstructorMV> GetAllPaginated(InstructorSearchMV search,int page = 1, int pagesize = 10);
    }
}
