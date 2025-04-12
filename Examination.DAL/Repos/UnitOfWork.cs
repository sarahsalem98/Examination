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
        public IStudentRepo StudentRepo { get; private set; }
        public IUserRepo UserRepo { get; private set; }
        public IBranchRepo BranchRepo { get; private set; }
        public IDepartmentRepo DepartmentRepo { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            StudentRepo = new StudentRepo(_db);
            UserRepo = new UserRepo(_db);
            BranchRepo = new BranchRepo(_db);
            DepartmentRepo = new DepartmentRepo(_db);
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
