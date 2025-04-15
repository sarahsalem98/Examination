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
    internal class InstructorRepo : Repo<Instructor>, IInstructorRepo
    {
        private readonly AppDbContext db;
        public InstructorRepo(AppDbContext _db) : base(_db)
        {
            db = _db;
        }
        public int UpdateCourses(List<InstructorCourse> newInstructorCourses, int instructorId)
        {
            var res = 0;
            var instructor = db.Instructors.Include(i => i.InstructorCourses).FirstOrDefault(i => i.Id == instructorId);
            foreach (var item in newInstructorCourses)
            {
                var existing = db.InstructorCourses.Include(s=>s.Instructor).ThenInclude(s=>s.User)
                                 .FirstOrDefault(ic =>
                                     ic.InstructorId != instructorId &&
                                     ic.Instructor.User.Status!=-1&&
                                     ic.DepartmentBranch.BranchId == item.DepartmentBranch.BranchId &&
                                     ic.DepartmentBranch.DepartmentId==item.DepartmentBranch.DepartmentId&&
                                     ic.CourseId == item.CourseId);

                if (existing != null)
                {
                    return 0;
                }
            }

            db.InstructorCourses.RemoveRange(instructor.InstructorCourses);
            db.InstructorCourses.AddRange(newInstructorCourses);
            res = db.SaveChanges();
            return res;

        }
        public List<InstructorCourse> GetInstructorCoursesWithDepartmentBranchId(List<InstructorCourse> newInstructorCourses)
        {
            var res = new List<InstructorCourse>();
            foreach (var item in newInstructorCourses)
            {
                var departmentBranchId = db.DepartmentBranches.FirstOrDefault(db => db.BranchId == item.DepartmentBranch.BranchId && db.DepartmentId == item.DepartmentBranch.DepartmentId)?.Id;
                if (departmentBranchId != null)
                {
                    bool alreadyAssigned = db.InstructorCourses.Include(s => s.Instructor).ThenInclude(s => s.User).Any(ic =>
                              ic.DepartmentBranch.BranchId == item.DepartmentBranch.BranchId &&
                              ic.Instructor.User.Status != -1 &&
                              ic.DepartmentBranch.DepartmentId == item.DepartmentBranch.DepartmentId &&
                            ic.CourseId == item.CourseId);

                    if (!alreadyAssigned)
                    
                    {
                        var instructorCourse = new InstructorCourse
                        {
                            CourseId = item.CourseId,
                            DepartmentBranchId = (int)departmentBranchId,
                            InstructorId = item.InstructorId
                        };
                        res.Add(instructorCourse);

                    }
                    else
                    {
                        return res;
                    }
                     
                }

            }
            return res;

        }
    }
}
