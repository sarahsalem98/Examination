using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class ExamStudentAnswerRepo : Repo<ExamStudentAnswer>, IExamStudentAnswerRepo
    {
        private readonly AppDbContext _db;
        public ExamStudentAnswerRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public int InsertStudentSingleAnswer(int StudentId, int QId, int GeneratedExamId, string StdAnswer)
        {
            try
            {
                var res = _db.Database.ExecuteSqlRaw(
"EXEC Exam.sp_SubmitSingleStudentAnswer @StudentId, @GeneratedExamId, @GeneratedExamQsId, @StdAnswer",
new SqlParameter("@StudentId", StudentId),
new SqlParameter("@GeneratedExamId", GeneratedExamId),
new SqlParameter("@GeneratedExamQsId", QId),
new SqlParameter("@StdAnswer", StdAnswer)
);
                return 1;
            }
            catch
            {
                return -1;
            }
;

        }
        public int UpdateStudentSingleAnswer(int StudentId, int QId, int GeneratedExamId, string StdAnswer)
        {
            try
            {
                var res = _db.Database.ExecuteSqlRaw(
                    "EXEC Exam.sp_UpdateSingleStudentAnswer @StudentId, @GeneratedExamId, @GeneratedExamQsId, @StdAnswer",
                    new SqlParameter("@StudentId", StudentId),
                    new SqlParameter("@GeneratedExamId", GeneratedExamId),
                    new SqlParameter("@GeneratedExamQsId", QId),
                    new SqlParameter("@StdAnswer", StdAnswer)
                );

                return 1;
            }
            catch
            {
                return -1; // SQL crash
            }
        }

    }

}
