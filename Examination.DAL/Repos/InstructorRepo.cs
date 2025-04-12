using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    internal class InstructorRepo:Repo<Instructor>,IInstructorRepo
    {
        private readonly AppDbContext db;
        public  InstructorRepo(AppDbContext _db):base(_db)
        {
            db = _db;
        }
    }
}
