﻿
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
        public PaginatedData<StudentMV> GetStudentsByInstructorPaginated(int Instructor_Id,StudentSearchMV studentSearch, int PageSize = 10, int Page = 1);
        StudentMV GetProfile(int userId);
        public StudentMV GetStudentCoursesWithInstructor(int Student_Id, int Instructor_Id);
        public int? GetStudentGrade(int Courseid);
        int UpdateProfile(StudentUpdateProfileMV student);
    
    }
}
