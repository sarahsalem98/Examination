namespace Examination.PL.IBL
{
    public interface IGeneratedExamService
    {
        public int GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ, int CreatedBy, DateOnly TakenDate, TimeOnly takenTime);
    }
}
