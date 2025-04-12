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
        public IInstructorRepo InstructorRepo { get; }
        public int Save();
    }
}
