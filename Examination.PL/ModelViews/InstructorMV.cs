using Examination.DAL.Entities;
using System.Diagnostics.Contracts;

namespace Examination.PL.ModelViews
{
    public class InstructorMV
    {
        public int Id { get; set; }

        public bool IsExternal { get; set; }

        public int UserId { get; set; }

        public List<InstructorCourseMV> InstructorCourses { get; set; } = new List<InstructorCourseMV>();
        public UserMV? User { get; set; }
    }
    public class InstructorSearchMV
    {
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? CourseId { get; set; }
        public int? status { get; set; }
        public bool ? IsExternal { get; set; }

    }

}
