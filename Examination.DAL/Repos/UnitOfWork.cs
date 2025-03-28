﻿using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
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
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            StudentRepo = new StudentRepo(_db);
        }
        public int Save()
        {
            return _db.SaveChanges();
        }
    }
   
}
