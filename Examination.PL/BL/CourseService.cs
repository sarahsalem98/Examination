using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using System.Drawing.Printing;

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
        public List<CourseMV> GetCourseByInstructor(int Instructor_Id)
        {
            try
            {
                List< CourseMV> courseMV = new List<CourseMV>();
                List<Course> data=unitOfWork.CourseRepo.GetAll(c=>c.InstructorCourses.Select(c=>c.Instructor.UserId).Contains(Instructor_Id),
                    "InstructorCourses").Where(c=>c.Status==(int)Status.Active).ToList();
                courseMV=mapper.Map<List< CourseMV>>(data);
                return courseMV;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in fetch Courses ");
                return null;

            }
        }
        public PaginatedData<CourseMV> GetCourseByInstructorPaginated(int Instructor_Id, CourseSearchMV courseSearch, int Page = 1, int PageSize = 10)
        {
            try
            {
                List<CourseMV> courseMV = new List<CourseMV>();


                List<Course> data = unitOfWork.CourseRepo.GetAll(
    c => c.Status == (int)Status.Active &&
         c.InstructorCourses.Any(ic =>
             ic.Instructor.UserId == Instructor_Id && 
             (courseSearch.CourseId == null || ic.CourseId == courseSearch.CourseId) &&
             (courseSearch.DepartmentId == null || ic.DepartmentBranch.DepartmentId == courseSearch.DepartmentId) &&
             (courseSearch.BranchId == null || ic.DepartmentBranch.BranchId == courseSearch.BranchId)
         ) &&
         (string.IsNullOrEmpty(courseSearch.Name) ||
         c.Name.ToLower().Trim().Contains(courseSearch.Name.ToLower().Trim()))
    , "InstructorCourses,InstructorCourses.Instructor,InstructorCourses.DepartmentBranch.Department,InstructorCourses.DepartmentBranch.Branch"
).ToList();
                courseMV = mapper.Map<List<CourseMV>>(data);
                int TotalCounts = courseMV.Count();
                if (TotalCounts > 0)
                {
                    courseMV = courseMV.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
                PaginatedData<CourseMV> paginatedData = new PaginatedData<CourseMV>
                {
                    Items = courseMV,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in fetch Courses ");
                return null;

            }
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

        public List<CourseMV> GetCourseByStatus(int status)
        {
            try
            {
                var courses = unitOfWork.CourseRepo.GetAll(c => c.Status == status);
                if (courses == null)
                {
                    return null;
                }
                else
                {
                    var coursesMV = mapper.Map<List<CourseMV>>(courses);
                    return coursesMV;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in fetch Courses in status ");
                return null;
            }
        }
    }
}
