using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos.IRepos
{
    public interface IUnitOfWork
    {
        public IStudentRepo StudentRepo { get; }
        public IUserRepo UserRepo { get; }
        public IBranchRepo BranchRepo { get; }
        public IDepartmentRepo DepartmentRepo { get; }
<<<<<<< HEAD
        public IInstructorRepo InstructorRepo { get; }
=======
        public IDepartmentBranch DepartmentBranchRepo { get; }
        public IUserTypeRepo UserTypeRepo { get; }
>>>>>>> 0385df70ff60e66a461d5dc372462dee9dc46ccc
        public int Save();
        public IDbContextTransaction BeginTransaction();
        

    }
}
