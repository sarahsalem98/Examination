using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IInstructorCourseService
    {
        public PaginatedData<InstructorCourseMV> GetCourseByInstructorPaginated(int Instructor_Id, CourseSearchMV courseSearch, int Page = 1, int PageSize = 10);
        public int CompleteCourse(int DepartmentBranchId,int instructor_id,int course_id);
    }
}
