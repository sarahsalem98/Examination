using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.ModelViews;

namespace Examination.PL.IBL;

public interface ICourseService
{
    public List<CourseMV> GetCoursesByDeaprtment(int id);
    public List<CourseMV> GetCourseByStatus(int status);
    public CourseMV GetCourseByID(int id);

    public int Add(CourseMV course);
    public PaginatedData<CourseMV> GetAllPaginated(CourseSearchMV courseSerach, int PageSize = 10, int Page = 1);

    public int Update(CourseMV course);
    public List<CourseMV> GetCourseByInstructor(int Instructor_Id);
    public int ChangeStatus(int id, int status);

}
