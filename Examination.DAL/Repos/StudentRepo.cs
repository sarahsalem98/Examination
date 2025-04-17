using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class StudentRepo : Repo<Student>, IStudentRepo
    {
        private readonly AppDbContext _db;
        public StudentRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public int AddStudentCoursesAccordingToDepartmentId(int StudentId, int DepartmentId)
        {
            var student = _db.Students.Include(s => s.StudentCourses).FirstOrDefault(s => s.Id == StudentId);
            if (student != null && student.StudentCourses.Count()==0)
            {
                var courses = _db.Courses.Where(c => c.CourseDepartments.Any(cd => cd.DepartmentId == DepartmentId)).ToList();
                foreach (var course in courses)
                {
                    if (!student.StudentCourses.Any(sc => sc.CourseId == course.Id))
                    {
                        student.StudentCourses.Add(new StudentCourse { CourseId = course.Id });
                    }
                }
                return _db.SaveChanges();
            }
            return 0;
        }
        public int UpdateStudentCoursesAccordingToDepartmentId(int StudentId, int DepartmentId)
        {
            var student = _db.Students.Include(s => s.StudentCourses).FirstOrDefault(s => s.Id == StudentId);
            var DoesFinsishedAtLeastOneCourse = _db.StudentCourses.Any(sc => sc.StudentId == StudentId && sc.FinalGradePercent != null);
            if (student != null & !DoesFinsishedAtLeastOneCourse)
            {
                var courses = _db.Courses.Where(c => c.CourseDepartments.Any(cd => cd.DepartmentId == DepartmentId)).ToList();
                _db.StudentCourses.RemoveRange(student.StudentCourses);
                student.StudentCourses.Clear();
                foreach (var course in courses)
                {
                    if (!student.StudentCourses.Any(sc => sc.CourseId == course.Id))
                    {
                        student.StudentCourses.Add(new StudentCourse { CourseId = course.Id });
                    }
                }
                return _db.SaveChanges();
            }
            return 0;
        }
        public int DoesStudentFinishedAtLeastOneCourse(int StudentId)
        {
            var DoesFinsishedAtLeastOneCourse = _db.StudentCourses.Any(sc => sc.StudentId == StudentId && sc.FinalGradePercent != null);
            if (DoesFinsishedAtLeastOneCourse)
            {
                return 1;
            }
            return 0;
        }


    }

}
