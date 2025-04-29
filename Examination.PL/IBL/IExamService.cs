using Examination.DAL.Entities;
using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IExamService
    {
        #region AdminExam
        public PaginatedData<ExamMV> GetAllPaginated(ExamSearchMV SearchModel, int PageSize = 10, int Page = 1);
        public ExamMV GetById(int id);
        public int ChnageStatus(int id, int status);
        public int Add(ExamMV exam);
        public int Update(ExamMV exam);

        #endregion
        public List<ExamMV> GetByStatus(int? status=null);
        public List<ExamMV> GetByInstructorDepartmentBranch(int instructor_id,  int department_id, int branch_id);

        #region OnGoingExam
        int SubmitQuestionAnswer(int StudentId, int QId, int GeneratedExamId, string StdAnswer);
   
        #endregion


    }
}
