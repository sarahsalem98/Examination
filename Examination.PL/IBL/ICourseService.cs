using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface ICourseService
    {
        public List<CourseMV> GetCoursesByDeaprtment(int id);
        public List<CourseMV> GetCourseByStatus(int status);
        public List<CourseMV> GetCourseByInstructor(int Instructor_Id);

    }
}
