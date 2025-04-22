using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class GeneratedExamRepo : Repo<GeneratedExam>, IGeneratedExamRepo
    {
        public GeneratedExamRepo(AppDbContext db) : base(db)
        {

        }
    }
}
