using Examination.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos.IRepos
{
    public interface IInstructorRepo:IRepo<Instructor>
    {
        public int UpdateCourses(List<InstructorCourse> newInstructorCourses, int instructorId);
        public List<InstructorCourse> GetInstructorCoursesWithDepartmentBranchId(List<InstructorCourse> newInstructorCourses);
    }
}
