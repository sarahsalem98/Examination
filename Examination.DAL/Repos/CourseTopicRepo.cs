﻿using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos;

public class CourseTopicRepo : Repo<CourseTopic>, ICourseTopicRepo
{
    private readonly AppDbContext _db;
    public CourseTopicRepo(AppDbContext db) : base(db)
    {
        _db = db;
    }
}
