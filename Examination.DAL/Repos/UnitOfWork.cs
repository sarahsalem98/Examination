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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;


        private IStudentRepo _studentRepo;
        private IUserRepo _userRepo;
        private IBranchRepo _branchRepo;
        private IDepartmentRepo _departmentRepo;
        private IInstructorRepo _instructorRepo;
        private IUserTypeRepo _userTypeRepo;
        private IExamRepo _examRepo;
        private IDepartmentBranch _departmentBranchRepo;
        private ICourseRepo _courseRepo;
        private IExamQuestionRepo _examQuestionRepo;
        private IGeneratedExamQRepo _generatedExamQRepo;
        private IInstructorCourseRepo _instructorCourseRepo;
        private IGeneratedExamRepo _generatedExamRepo;
        private ICourseDepartmentRepo _courseDepartmentRepo;
        private ITopicRepo _topicRepo;
        private ICourseTopicRepo _courseTopicRepo;
        private IStudentCourseRepo _studentCourseRepo;
        public IStudentRepo StudentRepo => _studentRepo ??= new StudentRepo(_db);
        public IUserRepo UserRepo => _userRepo ??= new UserRepo(_db);
        public IBranchRepo BranchRepo => _branchRepo ??= new BranchRepo(_db);
        public IDepartmentRepo DepartmentRepo => _departmentRepo ??= new DepartmentRepo(_db);
        public IInstructorRepo InstructorRepo => _instructorRepo ??= new InstructorRepo(_db);
        public IUserTypeRepo UserTypeRepo => _userTypeRepo ??= new UserTypeRepo(_db);
        public IExamRepo ExamRepo => _examRepo ??= new ExamRepo(_db);
        public IDepartmentBranch DepartmentBranchRepo => _departmentBranchRepo ??= new DepartmentBranchRepo(_db);
        public ICourseRepo CourseRepo => _courseRepo ??= new CourseRepo(_db);
        public IExamQuestionRepo ExamQuestionRepo => _examQuestionRepo ??= new ExamQuestionRepo(_db);
        public IGeneratedExamQRepo GeneratedExamQRepo => _generatedExamQRepo ??= new GeneratedExamQRepo(_db);
        public IInstructorCourseRepo InstructorCourseRepo => _instructorCourseRepo ??= new InstructorCourseRepo(_db);
        public IGeneratedExamRepo GeneratedExamRepo => _generatedExamRepo ??= new GeneratedExamRepo(_db);
        public ICourseDepartmentRepo CourseDepartmentRepo => _courseDepartmentRepo ??= new CourseDepartmentRepo(_db);
        public ITopicRepo TopicRepo => _topicRepo ??= new TopicRepo(_db);
        public ICourseTopicRepo CourseTopicRepo => _courseTopicRepo ??= new CourseTopicRepo(_db);
        public IStudentCourseRepo StudentCourseRepo => _studentCourseRepo ??= new StudentCourseRepo(_db);


        public UnitOfWork(AppDbContext db)
        {

            _db = db;
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
