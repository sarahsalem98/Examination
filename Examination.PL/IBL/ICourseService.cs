using Examination.PL.ModelViews;

namespace Examination.PL.IBL;

    public interface ICourseService
    {
        public List<CourseMV> GetCoursesByDeaprtment(int id);
        public List<CourseMV> GetCourseByStatus(int status);
    }
    public int Add(CourseMV course);
    public PaginatedData<CourseMV> GetAllPaginated(string searchName, int PageSize = 10, int Page = 1);

    public int Update(CourseMV course);
}
