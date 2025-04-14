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

        //public PaginatedData<ExamMV> GetAllPaginated(ExamSearchMV studentSearch, int PageSize = 10, int Page = 1)
        //{

        //    try
        //    {

        //        List<ExamMV> list = new List<ExamMV>();
        //        List<Exam> data = _unitOfWork.ExamRepo.GetAll(
        //                   s => (studentSearch.TrackType == null || s.TrackType == studentSearch.TrackType) &&
        //                   (studentSearch.BranchId == null || s.DepartmentBranch.BranchId == studentSearch.BranchId) &&
        //                   (studentSearch.Status != (int)Status.Deleted ? s.User.Status != (int)Status.Deleted : s.User.Status == (int)Status.Deleted) &&
        //                   (studentSearch.Status == null || s.User.Status == studentSearch.Status) &&
        //                   (studentSearch.DepartmentId == null || studentSearch.DepartmentId == s.DepartmentBranch.DepartmentId) &&
        //                   (String.IsNullOrEmpty(studentSearch.Name) ||
        //                   (!String.IsNullOrEmpty(s.User.FirstName) && s.User.FirstName.ToLower().Trim().Contains(studentSearch.Name)) ||
        //                   (!String.IsNullOrEmpty(s.User.LastName) && s.User.LastName.ToLower().Trim().Contains(studentSearch.Name)))


        //            , "User,DepartmentBranch.Department,DepartmentBranch.Branch").OrderByDescending(s => s.User.CreatedAt).ToList();
        //        studentMVs = _mapper.Map<List<StudentMV>>(data);
        //        int TotalCounts = studentMVs.Count();
        //        if (TotalCounts > 0)
        //        {
        //            studentMVs = studentMVs.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

        //        }
        //        PaginatedData<StudentMV> paginatedData = new PaginatedData<StudentMV>
        //        {
        //            Items = studentMVs,
        //            TotalCount = TotalCounts,
        //            PageSize = PageSize,
        //            CurrentPage = Page
        //        };
        //        return paginatedData;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "error occuired while getting all exams");
        //    }
        //}
    }
}
