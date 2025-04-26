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
                this.AddCourseToStudentsByDepartments(crs.Id, course.DepartmentsIds);
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
                List<CourseMV> courseMV = new List<CourseMV>();
                List<Course> data = _unitOfWork.CourseRepo.GetAll(c => c.InstructorCourses.Select(c => c.Instructor.UserId).Contains(Instructor_Id),
                    "InstructorCourses").Where(c => c.Status == (int)Status.Active).ToList();
                courseMV = _mapper.Map<List<CourseMV>>(data);
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
                courseExist.ImgUrl = course.ImgUrl;

                var courseDeptsExists = courseExist.CourseDepartments.ToList();
                var CourseDeptIdsExist = courseDeptsExists.Select(S => S.DepartmentId).ToList();
                var NewDeptIds = course.DepartmentsIds;

                var DeptIdsCoursesNeededToAdd = NewDeptIds
                                 .Where(id => !CourseDeptIdsExist.Contains(id))
                                 .ToList();

                var DeptIdsCoursesNeededToRemove = CourseDeptIdsExist
                    .Where(id => !NewDeptIds.Contains(id))
                    .ToList();

                if (CourseDeptIdsExist != NewDeptIds)
                {
                    _unitOfWork.CourseDepartmentRepo.RemoveRange(courseDeptsExists);
                    foreach (var dep in NewDeptIds)
                    {
                        var courseDept = new CourseDepartment() { CourseId = courseExist.Id, DepartmentId = dep };
                        _unitOfWork.CourseDepartmentRepo.Insert(courseDept);
                    }

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


                this.AddCourseToStudentsByDepartments(courseExist.Id, DeptIdsCoursesNeededToAdd);
                this.RemoveCourseFromStudentByDepartments(courseExist.Id, DeptIdsCoursesNeededToRemove);

                courseExist.UpdatedAt = DateTime.Now;
                courseExist.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
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

        public PaginatedData<CourseMV> GetCoursesByStudent( string name, string userIdString, int PageSize = 8,int Page = 1)
        {
            try
            {
                

                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                {
                    _logger.LogWarning("UserId is not found or invalid in the HttpContext.");
                    return new PaginatedData<CourseMV> { Items = new List<CourseMV>(), TotalCount = 0 };
                }

                var student = _unitOfWork.StudentRepo.FirstOrDefault(s => s.UserId == userId, "StudentCourses.Course");

                if (student == null)
                {
                    _logger.LogWarning("Student not found for UserId {userId}", userId);
                    return new PaginatedData<CourseMV> { Items = new List<CourseMV>(), TotalCount = 0 };
                }

                //var studentCourses = student.StudentCourses.Select(sc => sc.Course).ToList();

                //var courseMVs = _mapper.Map<List<CourseMV>>(studentCourses);

                var mappedCourses = student.StudentCourses.Select(sc => new
                {
                    Courses = _mapper.Map<CourseMV>(sc.Course),
                    CourseId = sc.Course.Id,
                    Grade = sc.FinalGradePercent
                });

                // Filter by name if provided
                if (!string.IsNullOrEmpty(name))
                {
                    mappedCourses = mappedCourses.Where(c => c.Courses.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int totalCount = mappedCourses.Count();
                var paginatedCourses = mappedCourses.Skip((Page - 1) * PageSize).Take(PageSize).ToList();


                _httpContextAccessor.HttpContext.Items["Grades"] =paginatedCourses.ToDictionary(p => p.CourseId, p => p.Grade);
                return new PaginatedData<CourseMV>
                {                    
                    Items = paginatedCourses.Select(p=>p.Courses).ToList(),
                    TotalCount = totalCount,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving courses.");
                return new PaginatedData<CourseMV> { Items = new List<CourseMV>(), TotalCount = 0 };
            }
        }


        public int AddCourseToStudentsByDepartments(int courseId, List<int> departmentIds)
        {
            try
            {

                if (departmentIds.Count() != 0)
                {
                    foreach (var id in departmentIds)
                    {
                        var studentsNeededToAddCourseTo = _unitOfWork.StudentRepo.GetAll(s => s.DepartmentBranch.Department.Id == id && s.EnrollmentDate.Value.Year == DateTime.Now.Year);
                        foreach (var student in studentsNeededToAddCourseTo)
                        {
                            var studentCourse = new StudentCourse()
                            {
                                StudentId = student.Id,
                                CourseId = courseId,

                            };

                            _unitOfWork.StudentCourseRepo.Insert(studentCourse);
                        }

                    }
                }

                return _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occurred while adding Course to Students ");
                return 0;
            }
        }

        public int RemoveCourseFromStudentByDepartments(int courseId, List<int> departmentIds)
        {
            try
            {
                if (departmentIds.Count() != 0)
                {
                    foreach (var id in departmentIds)
                    {
                        var students = _unitOfWork.StudentRepo.GetAll(s => s.DepartmentBranch.Department.Id == id && s.EnrollmentDate.Value.Year == DateTime.Now.Year, "StudentCourses");
                        var studentsNeededToRemoveFromCourse = students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == courseId &&sc.FinalGradePercent==null)).ToList();
                        if (studentsNeededToRemoveFromCourse.Count() != 0)
                        {
                            foreach (var student in studentsNeededToRemoveFromCourse)
                            {

                                var coursesToRemove = student.StudentCourses
                                                    .Where(sc => sc.CourseId == courseId && sc.FinalGradePercent == null)
                                                    .ToList();

                                if (coursesToRemove.Count > 0)
                                {
                                    _unitOfWork.StudentCourseRepo.RemoveRange(coursesToRemove);
                                }

                            }
                        }
                    }
                }
                return _unitOfWork.Save();
               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occurred while removing Course from Students ");
                return 0;
            }
        }
    }
}
