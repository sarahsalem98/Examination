using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
   public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IStudentRepo StudentRepo { get { return new StudentRepo(_db); } }
        public IUserRepo UserRepo { get { return new UserRepo(_db); } }
        public IBranchRepo BranchRepo { get { return new BranchRepo(_db); } }
        public IDepartmentRepo DepartmentRepo { get { return new DepartmentRepo (_db); } }
        public IInstructorRepo InstructorRepo { get { return new InstructorRepo(_db); }  }
        public IUserTypeRepo UserTypeRepo { get { return new UserTypeRepo(_db); }  }
        public IExamRepo ExamRepo { get { return new ExamRepo(_db); }  }
        public IDepartmentBranch DepartmentBranchRepo { get { return new DepartmentBranchRepo(_db); } } 
        public ICourseRepo CourseRepo { get { return new CourseRepo(_db); } }
        public IExamQuestionRepo ExamQuestionRepo { get { return new ExamQuestionRepo(_db); } }
        public IInstructorCourseRepo InstructorCourseRepo { get { return new InstructorCourseRepo(_db); } }
        public UnitOfWork(AppDbContext db)
        {

            _db = db;
            //StudentRepo = new StudentRepo(_db);
            //UserRepo = new UserRepo(_db);
            //BranchRepo = new BranchRepo(_db);
            //DepartmentRepo = new DepartmentRepo(_db);
            //InstructorRepo = new InstructorRepo(_db);
            //UserTypeRepo = new UserTypeRepo(_db);
            //DepartmentBranchRepo = new DepartmentBranchRepo(_db);
            //UserTypeRepo = new UserTypeRepo(_db);
            //ExamRepo = new ExamRepo(_db);
        }
        public int Save()
        {
            return _db.SaveChanges();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

    }

}
