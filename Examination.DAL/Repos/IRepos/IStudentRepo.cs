using Examination.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos.IRepos
{
    public interface IStudentRepo : IRepo<Student>
    {
        public int AddStudentCoursesAccordingToDepartmentId(int StudentId, int DepartmentId);
        public int UpdateStudentCoursesAccordingToDepartmentId(int StudentId, int DepartmentId);
        public int DoesStudentFinishedAtLeastOneCourse(int StudentId);
    }
}
