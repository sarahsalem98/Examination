﻿using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IGeneratedExamService
    {
        public int GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ, int CreatedBy, DateOnly TakenDate, TimeOnly takenTime);
        public PaginatedData<GeneratedExamMV> GetAllPaginated(int instructor_id,GeneratedExamSearchMV search, int PageSize = 10, int Page = 1);
        public GeneratedExamMV GetByID(int GeneratedExamId);
        public int UpdateGeneratedExam(int GeneratedExamID, DateOnly TakenDate, TimeOnly takenTime);
        public List<GeneratedExamMV> CommingExam(string userIdString);

        public PaginatedData<GeneratedExamMV> GetPreviousExams(string userIdString, GeneratedExamSearchMV search, int pageSize = 10, int page = 1);

    }
}
