using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class UserTypeRepo : Repo<UserType>,IUserTypeRepo
    {
        public UserTypeRepo(AppDbContext db) : base(db)
        {
        }
    }
    
}
