using Examination.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos.IRepos
{
   public interface IExamStudentAnswerRepo:IRepo<ExamStudentAnswer>
    {
        public int InsertStudentSingleAnswer(int StudentId, int QId, int GeneratedExamId, string StdAnswer);
        public int UpdateStudentSingleAnswer(int StudentId, int QId, int GeneratedExamId, string StdAnswer);
        public int CorrectExam(int GeneratedExamId, int StudentId, string Type, int instructorCourseId, int MinSuccessPrecent);
    }
}
