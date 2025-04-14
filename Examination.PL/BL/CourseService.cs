using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CourseService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public int Add(CourseMV course)
        {
            int result = 0;
            try
            {
                var crs = _mapper.Map<Course>(course);
                _unitOfWork.CourseRepo.Insert(crs);
                result = _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public PaginatedData<CourseMV> GetAllPaginated(string searchName, int PageSize = 10, int Page = 1)
        {
            try
            {
                List<CourseMV> CourseMVs = new List<CourseMV>();
                List<Course> data = _unitOfWork.CourseRepo.GetAll().ToList();
                CourseMVs = _mapper.Map<List<CourseMV>>(data);
                int TotalCounts = CourseMVs.Count();
                if (TotalCounts > 0)
                {
                    CourseMVs = CourseMVs.Skip((Page - 1) * PageSize).Take(1).ToList();

                }
                PaginatedData<CourseMV> paginatedData = new PaginatedData<CourseMV>
                {
                    Items = CourseMVs,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving student data in admin area");
                return null;

            }
        }
    }
}
