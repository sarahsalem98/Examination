using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IExamQuestionService
    {
       public PaginatedData<ExamQuestionMV> GetAllExamsQuestions(ExamQuestionSearchMV searchMV, int PageSize = 10, int Page = 1);
       public int Add(ExamQuestionMV model);
       public int Update(ExamQuestionMV model);
       public ExamQuestionMV GetById(int id);
       public int ChangeStatus(int id, int status);


    }
}
