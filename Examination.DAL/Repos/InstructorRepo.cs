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
            var instructor = db.Instructors.Include(i => i.InstructorCourses).ThenInclude(i=>i.DepartmentBranch).FirstOrDefault(i => i.Id == instructorId);
            if (instructor == null)
            {
                return 0;
            }
            var coursesToRemove = instructor.InstructorCourses
            .Where(ic => !newInstructorCourses.Any(nc =>
                nc.DepartmentBranch?.BranchId == ic.DepartmentBranch?.BranchId &&
                nc.DepartmentBranch?.DepartmentId == ic.DepartmentBranch?.DepartmentId &&
                nc.CourseId == ic.CourseId))
            .ToList();
            foreach (var newCourse in newInstructorCourses)
            {
                var existing = instructor.InstructorCourses
                    .FirstOrDefault(ic =>ic.Id==newCourse.Id);

                if (existing != null)
                {
                    existing.CourseId = newCourse.CourseId;
                    existing.DepartmentBranchId =db.DepartmentBranches.FirstOrDefault(d=>d.DepartmentId==newCourse.DepartmentBranch.DepartmentId&&d.BranchId==newCourse.DepartmentBranch.BranchId).Id ;
                    existing.TotalStudents = db.Departments.FirstOrDefault(d=>d.Id==newCourse.DepartmentBranch.DepartmentId).Capacity;
                    existing.StartDate = newCourse.StartDate;
                    existing.EndDate = newCourse.EndDate;

                    
                }
                else
                {
                 
                    var NewCourse = new InstructorCourse
                    {
                        CourseId = newCourse.CourseId,
                        DepartmentBranchId = db.DepartmentBranches.FirstOrDefault(d => d.DepartmentId == newCourse.DepartmentBranch.DepartmentId && d.BranchId == newCourse.DepartmentBranch.BranchId).Id,
                        InstructorId = instructor.Id,
                        TotalStudents = db.Departments.FirstOrDefault(d => d.Id == newCourse.DepartmentBranch.DepartmentId).Capacity,
                        StartDate = newCourse.StartDate,
                        EndDate = newCourse.EndDate,
                    };
                    instructor.InstructorCourses.Add(NewCourse);
                }
            }
        
            db.InstructorCourses.RemoveRange(coursesToRemove);
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
                            InstructorId = item.InstructorId,
                            TotalStudents = db.Departments.FirstOrDefault(d=>d.Id==item.DepartmentBranch.DepartmentId).Capacity,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
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
