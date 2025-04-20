using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Http;

namespace Examination.PL.BL
{
    public class ExamService:IExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExamService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExamService(IUnitOfWork unitOfWork, IMapper mapper,ILogger<ExamService> logger,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

        }
        #region AdminExam

        public int Add(ExamMV exam)
        {
            try
            {
                var examExsist = _unitOfWork.ExamRepo.FirstOrDefault(e => e.CourseId == exam.CourseId && e.Type == exam.Type);
                if (examExsist != null)
                {
                    return -1;
                }
                var NewExam= _mapper.Map<Exam>(exam);

                NewExam.Name = exam.Name;
                NewExam.Type = exam.Type;
                NewExam.Duration=exam.Duration;
                NewExam.Description = exam.Description;
                NewExam.CourseId = exam.CourseId;
                NewExam.CreatedAt =DateTime.Now;
                NewExam.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                NewExam.Status = (int)Status.Active;
                _unitOfWork.ExamRepo.Insert(NewExam);
                var res = _unitOfWork.Save();
                if(res>0)
                {
                    return 1;
                }else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while Adding exam");
                return 0;
            }
        }
        public int Update(ExamMV exam)
        {
            try
            {
                var OldExam=_unitOfWork.ExamRepo.FirstOrDefault(e=>e.Id==exam.Id, "Course");
                if (OldExam == null)
                {
                    return -2;
                }
                var examExsist = _unitOfWork.ExamRepo.FirstOrDefault(e => e.CourseId == exam.CourseId && e.Type == exam.Type&&e.Id!=OldExam.Id);
                if (examExsist != null)
                {
                    return -1;
                }
                OldExam.Name = exam.Name;
                OldExam.Type = exam.Type;
                OldExam.Duration = exam.Duration;
                OldExam.Description = exam.Description;
                OldExam.CourseId = exam.CourseId;
                OldExam.UpdatedAt = DateTime.Now;
                OldExam.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                OldExam.Status = (int)Status.Active;
                
                _unitOfWork.ExamRepo.Update(OldExam);
                var res = _unitOfWork.Save();
                if (res > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while Updating exam");
                return 0;
            }
        }
        public int ChnageStatus(int id,int status)
        {
            try
            {
                var Exam = _unitOfWork.ExamRepo.FirstOrDefault(d => d.Id == id);
                if (Exam == null)
                {
                    return -1;
                }
                else
                {
                    Exam.Status = status;
                    _unitOfWork.ExamRepo.Update(Exam);
                    var res = _unitOfWork.Save();
                    if (res > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while Updating exam");
                return 0;
            }
        }
        public ExamMV GetById(int id)
        {
            try
            {
                ExamMV examMV=new ExamMV();

                var  exam=_unitOfWork.ExamRepo.FirstOrDefault(d=>d.Id==id, "Course");
                if(exam == null)
                {
                    return examMV;
                }else
                {
                    examMV = _mapper.Map<ExamMV>(exam);
                    return examMV;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while Updating exam");
                return null;
            }
        }
        public PaginatedData<ExamMV> GetAllPaginated(ExamSearchMV SearchModel, int PageSize = 10, int Page = 1)
        {

            try
            {
                List<ExamMV> list  = new List<ExamMV>();
                List<Exam> data = _unitOfWork.ExamRepo.GetAll(
                    item =>
                        (String.IsNullOrEmpty(SearchModel.Type) || item.Type == SearchModel.Type) &&
                        (SearchModel.CourseId == null || item.CourseId == SearchModel.CourseId) &&
                        (SearchModel.Status != (int)Status.Deleted ? item.Status != (int)Status.Deleted : item.Status == (int)Status.Deleted) &&
                        (SearchModel.Status == null || item.Status == SearchModel.Status) &&
                        (
                            String.IsNullOrEmpty(SearchModel.Name) ||
                            (!String.IsNullOrEmpty(item.Name) && item.Name.ToLower().Trim().Contains(SearchModel.Name.ToLower().Trim()))
                        )
                    , "Course"
                ).OrderByDescending(item => item.CreatedAt).ToList();
                list = _mapper.Map<List<ExamMV>>(data);
                int TotalCounts = list.Count();
                if (TotalCounts > 0)
                {
                    list = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
                PaginatedData<ExamMV> paginatedData = new PaginatedData<ExamMV>
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
        #endregion

     
    }
}
