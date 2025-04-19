
using Examination.PL.ModelViews;


namespace Examination.PL.IBL
{
    public interface IStudentService
    {

        public int Add(StudentMV student);
        public PaginatedData<StudentMV> GetAllPaginated(StudentSearchMV studentSearch ,int PageSize = 10, int Page=1);
        public StudentMV GetById(int id);
        public int Update(StudentMV student);
        public int ChangeStatus(int id, int status);
        public PaginatedData<StudentMV> GetStudentsByInstructorPaginated(StudentSearchMV studentSearch, int PageSize = 10, int Page = 1);
        public StudentMV GetStudentCoursesWithInstructor(int id);

    }
}
