using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IGeneratedExamQService
    {
        public List<GeneratedExamQMV> GetGeneratedExam(int GeneratedExam_id);
    }
}
