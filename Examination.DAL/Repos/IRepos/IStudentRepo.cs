﻿using Examination.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos.IRepos
{
   public interface IStudentRepo: IRepo<Student>
    {
        public void Update(Student student);
    }
}
