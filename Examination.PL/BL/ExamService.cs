using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

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
    }
}
