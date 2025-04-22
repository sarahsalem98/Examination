using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using System.Drawing.Printing;

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
                course.Status = (int)Status.Active;
                course.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                var crs = _mapper.Map<Course>(course);
                crs.CreatedAt = DateTime.Now;
                // Should be the id of the Logged User  
                _unitOfWork.CourseRepo.Insert(crs);
                _unitOfWork.Save();
                foreach (var dep in course.DepartmentsIds)
                {
                    _unitOfWork.CourseDepartmentRepo.Insert(new CourseDepartment
                    {
                        CourseId = crs.Id,
                        DepartmentId = dep
                    });
                }
                foreach (var topic in course.TopicsIds)
                {
                    _unitOfWork.CourseTopicRepo.Insert(new CourseTopic
                    {
                        CourseId = crs.Id,
                        TopicId = topic
                    });
                }
                result = _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error Occurred while adding new Course ");
                return 0;
            }
        }

        public PaginatedData<CourseMV> GetAllPaginated(CourseSearchMV courseSearch, int PageSize = 10, int Page = 1)
        {
            try
            {
                List<CourseMV> CourseMVs = new List<CourseMV>();
                List<Course> data = _unitOfWork.CourseRepo.GetAll(
                    s =>
                    //(courseSearch.BranchId == null || s.DepartmentBranch.BranchId == courseSearch.BranchId) &&
                    //(courseSearch.BranchId == null || s.InstructorCourses.Any(i=>i.DepartmentBranch.BranchId==courseSearch.BranchId)) &&

                    //1
                                       (courseSearch.BranchId == null ||
                    s.InstructorCourses != null &&
                    s.InstructorCourses.Any(i =>
                        i.DepartmentBranch != null &&
                        i.DepartmentBranch.BranchId == courseSearch.BranchId)) &&
                    //2
                    (courseSearch.Status != (int)Status.Deleted ? s.Status != (int)Status.Deleted : s.Status == (int)Status.Deleted) &&
                    //3
                    (courseSearch.Status == null || s.Status == courseSearch.Status) &&

                    //4
                    (courseSearch.DepartmentId == null || courseSearch.DepartmentId.Value == s.CourseDepartments.FirstOrDefault(c =>
                                             c.DepartmentId == courseSearch.DepartmentId.Value).DepartmentId) &&

                    //5
                    (String.IsNullOrEmpty(courseSearch.Name) ||
                    (!String.IsNullOrEmpty(s.Name) && s.Name.ToLower().Trim().Contains(courseSearch.Name)))
                    , "CourseDepartments,InstructorCourses.DepartmentBranch,CourseTopics").OrderByDescending(s => s.CreatedAt).ToList();
                CourseMVs = _mapper.Map<List<CourseMV>>(data);
                int TotalCounts = CourseMVs.Count();
                if (TotalCounts > 0)
                {
                    CourseMVs = CourseMVs.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

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
      

        public List<CourseMV> GetCourseByInstructor(int Instructor_Id)
        {
            try
            {
                List< CourseMV> courseMV = new List<CourseMV>();
                List<Course> data=_unitOfWork.CourseRepo.GetAll(c=>c.InstructorCourses.Select(c=>c.Instructor.UserId).Contains(Instructor_Id),
                    "InstructorCourses").Where(c=>c.Status==(int)Status.Active).ToList();
                courseMV=_mapper.Map<List< CourseMV>>(data);
                return courseMV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fetch Courses ");
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
                var courseExist = _unitOfWork.CourseRepo.FirstOrDefault(c => c.Id == course.Id, "CourseDepartments,CourseTopics");
                if (courseExist == null)
                    return result = -1;
                // Updating
                courseExist.Name = course.Name;
                courseExist.Description = course.Description;
                courseExist.Hours = course.Hours;

                var courseDepts = courseExist.CourseDepartments.ToList();
                var CourseDeptIds = courseDepts.Select(S => S.DepartmentId).ToList();

                if (CourseDeptIds != course.DepartmentsIds)
                {
                    CourseDeptIds = course.DepartmentsIds;
                    _unitOfWork.CourseDepartmentRepo.RemoveRange(courseDepts);
                    foreach (var dep in CourseDeptIds)
                    {
                        var courseDept = new CourseDepartment() { CourseId = courseExist.Id, DepartmentId = dep };
                        _unitOfWork.CourseDepartmentRepo.Insert(courseDept);
                    }
                    ;
                }



                var courseTopic = courseExist.CourseTopics.ToList();
                var courseTopicIds = courseTopic.Select(S => S.TopicId).ToList();
                if (courseTopicIds != course.TopicsIds)
                {
                    courseTopicIds = course.TopicsIds;
                    _unitOfWork.CourseTopicRepo.RemoveRange(courseTopic);
                    foreach (var topic in courseTopicIds)
                    {
                        var newCourseTopic = new CourseTopic() { CourseId = courseExist.Id, TopicId = topic };
                        _unitOfWork.CourseTopicRepo.Insert(newCourseTopic);
                    }
                    ;
                }





                courseExist.UpdatedAt = DateTime.Now;
                courseExist.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);



                courseExist.UpdatedAt = DateTime.Now;

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
                var course = _unitOfWork.CourseRepo.FirstOrDefault(i => i.Id == id, "CourseDepartments,CourseTopics");
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

        public int ChangeStatus(int id, int status)
        {
            try
            {
                var course = _unitOfWork.CourseRepo.FirstOrDefault(s => s.Id == id);
                if (course != null)
                {
                    course.Status = status;
                    course.UpdatedAt = DateTime.Now;
                    course.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    _unitOfWork.CourseRepo.Update(course);
                    return _unitOfWork.Save();
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while changing Course status ");
                return 0;
            }
        }
    }
}
