using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class ExamQuestionService:IExamQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExamQuestionService> _logger;
        public ExamQuestionService(IUnitOfWork unitOfWork, IMapper mapper ,ILogger<ExamQuestionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }
        //public PaginatedData<ExamQuestionMV> GetAllExamsQuestions(ExamQuestionSearchMV searchMV, int PageSize = 10, int Page = 1)
        //{
        //    try
        //    {
        //        List<ExamQuestionMV> list = new List<ExamQuestionMV>();
        //        List<ExamQ> data = _unitOfWork.ExamQuestionRepo.GetAll(
        //            item =>
        //                (searchMV.CourseId == null || item.Exam.CourseId == searchMV.CourseId) &&
        //                (searchMV.Status != (int)Status.Deleted ? item. != (int)Status.Deleted : item.Status == (int)Status.Deleted) &&
        //                (searchMV.Status == null || item.Status == searchMV.Status) &&
        //                (
        //                    String.IsNullOrEmpty(searchMV.Question) ||
        //                    (!String.IsNullOrEmpty(item.Question) && item.Question.ToLower().Trim().Contains(searchMV.Question.ToLower().Trim()))
        //                )
        //            , "Exam"
        //        ).OrderByDescending(item => item.CreatedAt).ToList();
        //        list = _mapper.Map<List<ExamQuestionMV>>(data);
        //        int TotalCounts = list.Count();
        //        if (TotalCounts > 0)
        //        {
        //            list = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
        //        }
        //        PaginatedData<ExamQuestionMV> paginatedData = new PaginatedData<ExamQuestionMV>
        //        {
        //            Items = list,
        //            TotalCount = TotalCounts,
        //            PageSize = PageSize,
        //            CurrentPage = Page
        //        };
        //        return paginatedData;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "error occuired while getting all exams");
        //        return null;
        //    }
        //}


    }
}
