using AutoMapper;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<InstructorService> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CourseService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<InstructorService> _logger, IHttpContextAccessor _httpContextAccessor)
        {

            unitOfWork = _unitOfWork;
            mapper = _mapper;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        public List<CourseMV> GetCoursesByDeaprtment(int id)
        {
            try
            {
                var courses = unitOfWork.CourseRepo.GetAll(
                    c => c.CourseDepartments.Select(d => d.DepartmentId).Contains(id),
                           "CourseDepartments");




                if (courses==null)
                {
                    return null;
                }
                else
                {
                    var coursesMV=mapper.Map<List<CourseMV>>(courses);
                    return coursesMV;
                }
             }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in fetch Courses ");
                return null;

            }
        }
    }
}
