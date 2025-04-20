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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExamQuestionService(IUnitOfWork unitOfWork, IMapper mapper ,ILogger<ExamQuestionService> logger,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

        }
        public PaginatedData<ExamQuestionMV> GetAllExamsQuestions(ExamQuestionSearchMV searchMV, int PageSize = 7, int Page = 1)
        {
            try
            {
                List<ExamQuestionMV> list = new List<ExamQuestionMV>();
                List<ExamQ> data = _unitOfWork.ExamQuestionRepo.GetAll(
                    item =>
                        (searchMV.CourseId == null || item.Exam.CourseId == searchMV.CourseId) &&
                        (searchMV.Status != (int)Status.Deleted ? item.Status != (int)Status.Deleted : item.Status == (int)Status.Deleted) &&
                        (searchMV.Status == null || item.Status == searchMV.Status) &&
                        (searchMV.QuestionType == null || item.QuestionType == searchMV.QuestionType) &&
                        (searchMV.Type == null || item.Exam.Type== searchMV.Type) 

                    , "Exam.Course"
                ).OrderByDescending(item => item.CreatedAt).ToList();
                list = _mapper.Map<List<ExamQuestionMV>>(data);
                int TotalCounts = list.Count();
                if (TotalCounts > 0)
                {
                    list = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
                }
                PaginatedData<ExamQuestionMV> paginatedData = new PaginatedData<ExamQuestionMV>
                {
                    Items = list,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while getting all exams");
                return null;
            }
        }

        public int Add(ExamQuestionMV model)
        {
            var result = 0;
            try
            {
                ExamQ examQ = _mapper.Map<ExamQ>(model);
                examQ.CreatedAt = DateTime.Now;
                examQ.Status = (int)Status.Active;
                examQ.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                _unitOfWork.ExamQuestionRepo.Insert(examQ);
                result= _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while adding new exam question");
                return 0;
            }
        }
        public int Update(ExamQuestionMV model)
        {
            var result = 0;
            try
            {
                ExamQ examQ = _unitOfWork.ExamQuestionRepo.GetById(model.Id);
                if (examQ == null)
                {
                    return 0;
                }
                examQ.Question = model.Question;
                examQ.QuestionType = model.QuestionType;
                examQ.Answers = model.Answers;
                examQ.RightAnswer = model.RightAnswer;
                examQ.Degree = model.Degree;
                examQ.ExamId = model.ExamId;
                examQ.UpdatedAt = DateTime.Now;
                examQ.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                _unitOfWork.ExamQuestionRepo.Update(examQ);
                result= _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while updating exam question");
                return 0;
            }
        }
        public ExamQuestionMV GetById(int id)
        {
            try
            {
                var examQ = _unitOfWork.ExamQuestionRepo.GetById(id);
                if (examQ == null)
                {
                    return null;
                }
                var result = _mapper.Map<ExamQuestionMV>(examQ);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while getting exam question by id");
                return null;

            }
        }
        public int ChangeStatus(int id, int status)
        {
            try
            {
                var examQ = _unitOfWork.ExamQuestionRepo.GetById(id);
                if (examQ == null)
                {
                    return 0;
                }
                examQ.Status = status;
                examQ.UpdatedAt = DateTime.Now;
                examQ.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                _unitOfWork.ExamQuestionRepo.Update(examQ);
                var result = _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while changing status of exam question");
                return 0;
            }
        }




    }
}
