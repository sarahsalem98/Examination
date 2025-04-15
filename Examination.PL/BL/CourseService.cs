using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CourseService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public int Add(CourseMV course)
        {
            int result = 0;
            try
            {
                var crs = _mapper.Map<Course>(course);
                crs.CreatedAt = DateTime.Now;
                // Should be the id of the Admin  
                if (crs.CreatedBy == 0)
                    crs.CreatedBy = 1;
                _unitOfWork.CourseRepo.Insert(crs);
                result = _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error Occurred while adding new Course ");
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

        public List<CourseMV> GetCoursesByDeaprtment(int id)
        {
            try
            {
                var courses = _unitOfWork.CourseRepo.GetAll(
                    c => c.CourseDepartments.Select(d => d.DepartmentId).Contains(id),
                           "CourseDepartments");




                if (courses == null)
                {
                    return null;
                }
                else
                {
                    var coursesMV = _mapper.Map<List<CourseMV>>(courses);
                    return coursesMV;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fetch Courses ");
                return null;

            }
        }

        public List<CourseMV> GetCourseByStatus(int status)
        {
            try
            {
                var courses = _unitOfWork.CourseRepo.GetAll(c => c.Status == status);
                if (courses == null)
                {
                    return null;
                }
                else
                {
                    var coursesMV = _mapper.Map<List<CourseMV>>(courses);
                    return coursesMV;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fetch Courses in status ");
                return null;
            }
        }

        public int Update(CourseMV course)
        {
            try
            {
                int result = 0;
                var courseExist = _unitOfWork.CourseRepo.FirstOrDefault(c => c.Id == course.Id);
                if (courseExist == null)
                    return result = -1;
                _mapper.Map(course, courseExist);
                courseExist.UpdatedAt = DateTime.Now;
                //courseExist.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                courseExist.UpdatedBy = 0;
                _unitOfWork.CourseRepo.Update(courseExist);
                result = _unitOfWork.Save();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "error occurred while updating Course data ");
                return 0;
            }
        }

        public CourseMV GetCourseByID(int id)
        {
            try
            {
                var course = _unitOfWork.CourseRepo.FirstOrDefault(i => i.Id == id);
                if (course == null)
                    return null;
                var courseMv = _mapper.Map<CourseMV>(course);
                return courseMv;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in fetch Courses ");
                return null;
            }
        }
    }
}
