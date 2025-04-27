using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class GeneratedExamRepo : Repo<GeneratedExam>, IGeneratedExamRepo
    {
        private readonly AppDbContext _db;
        public GeneratedExamRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public int GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ,int CreatedBy, DateOnly TakenDate, TimeOnly takenTime)
        {
            
          var res=  _db.Database.ExecuteSqlRaw("[Exam].[sp_Generate_Exam] @ExamId, @DepartmentId, @BranchId, @NumsTS, @NumsMCQ, @CreatedBy, @TakenDate, @TakenTime",
             new SqlParameter("@ExamId", ExamId),
            new SqlParameter("@DepartmentId", DepartmentId),
            new SqlParameter("@BranchId", BranchId),
            new SqlParameter("@NumsTS", NumsTS),
            new SqlParameter("@NumsMCQ", NumsMCQ),
            new SqlParameter("@CreatedBy", CreatedBy),
           new SqlParameter("@TakenDate", TakenDate),
           new SqlParameter("@TakenTime", takenTime));
            //if (res > 0)
            //    return 1;
            //else
            //    return 0;
            return 1;
         
        }
    }
}
