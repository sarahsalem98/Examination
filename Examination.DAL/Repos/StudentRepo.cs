using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class StudentRepo : Repo<Student>, IStudentRepo
    {
        private readonly AppDbContext _db;
        public StudentRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
       


}
    

}
