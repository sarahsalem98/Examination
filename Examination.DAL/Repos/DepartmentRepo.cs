using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
   public class DepartmentRepo:Repo<Department>, IDepartmentRepo
    {
        private readonly AppDbContext _db;
        public DepartmentRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
      
    }
}
