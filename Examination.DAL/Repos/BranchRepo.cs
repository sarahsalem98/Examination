using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class BranchRepo : Repo<Branch>, IBranchRepo
    {
        public BranchRepo(AppDbContext db) : base(db)
        {
        }
    }
}
