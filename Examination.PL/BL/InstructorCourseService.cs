using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class InstructorCourseService : IInstructorCourseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<InstructorService> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public InstructorCourseService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<InstructorService> _logger, IHttpContextAccessor _httpContextAccessor)
        {

            unitOfWork = _unitOfWork;
            mapper = _mapper;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        public PaginatedData<InstructorCourseMV> GetCourseByInstructorPaginated(int Instructor_Id, CourseSearchMV courseSearch, int Page = 1, int PageSize = 10)
        {
            try
            {
                List<InstructorCourseMV> instructorcourseMV = new List<InstructorCourseMV>();


                List<InstructorCourse> data = unitOfWork.InstructorCourseRepo.GetAll(ic=>ic.Instructor.UserId==Instructor_Id
                &&(courseSearch.CourseId==null||ic.CourseId==courseSearch.CourseId)
                &&(courseSearch.DepartmentId==null||ic.DepartmentBranch.DepartmentId==courseSearch.DepartmentId)
               && (courseSearch.BranchId == null || ic.DepartmentBranch.BranchId == courseSearch.BranchId)
               && (string.IsNullOrEmpty(courseSearch.Name) ||
                   ic.Course.Name.ToLower().Trim().Contains(courseSearch.Name.ToLower().Trim()))


         , "Course,Instructor,DepartmentBranch,DepartmentBranch.Department,DepartmentBranch.Branch").Where(ic=>ic.DepartmentBranch.Department.Status==(int)Status.Active&&ic.Course.Status== (int)Status.Active
        &&ic.DepartmentBranch.Branch.Status== (int)Status.Active).ToList();
                instructorcourseMV = mapper.Map<List<InstructorCourseMV>>(data);
                int TotalCounts = instructorcourseMV.Count();
                if (TotalCounts > 0)
                {
                    instructorcourseMV = instructorcourseMV.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
               ;

                PaginatedData<InstructorCourseMV> paginatedData = new PaginatedData<InstructorCourseMV>
                {
                    Items = instructorcourseMV,
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
    }
}
