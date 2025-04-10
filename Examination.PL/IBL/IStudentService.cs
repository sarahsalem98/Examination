
using Examination.PL.ModelViews;


namespace Examination.PL.IBL
{
    public interface IStudentService
    {

        public int Add(StudentMV student);
        public PaginatedData<StudentMV> GetAllPaginated(StudentSearchMV studentSearch ,int PageSize = 10, int Page=1);

    }
}
