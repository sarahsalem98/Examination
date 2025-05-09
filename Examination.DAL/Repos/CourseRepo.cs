﻿using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class CourseRepo : Repo<Course>, ICourseRepo
    {
        private readonly AppDbContext _db;
        public CourseRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }

}
