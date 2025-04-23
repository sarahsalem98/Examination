using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class StudentCourseRepo:Repo<StudentCourse>, IStudentCourseRepo
    {
        private readonly AppDbContext _db;
        public StudentCourseRepo(AppDbContext context) : base(context)
        {
            _db = context;

        }
    }
    
}
