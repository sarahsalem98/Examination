using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class ExamRepo : Repo<ExamRepo>, IExamRepo
    {
        public ExamRepo(Entities.AppDbContext db) : base(db)
        {
        }
    }
}
