using AutoMapper;
using BCrypt.Net;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using System.Linq;

namespace Examination.PL.BL
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentService> logger, IHttpContextAccessor httpContextAccessor)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        public PaginatedData<StudentMV> GetStudentsByInstructorPaginated(int Instructor_Id, StudentSearchMV studentSearch, int PageSize = 10, int Page = 1)
        {
            try
            {

                List<StudentMV> studentsMV = new List<StudentMV>();
                List<Student> data = _unitOfWork.StudentRepo.GetAll((s => s.DepartmentBranch.InstructorCourses.Any(c => c.Instructor.UserId == Instructor_Id) &&
                (studentSearch.DepartmentId == null || s.DepartmentBranch.DepartmentId == studentSearch.DepartmentId) &&
               (studentSearch.BranchId == null || s.DepartmentBranch.BranchId == studentSearch.BranchId) &&
               (studentSearch.TrackType == null || s.TrackType == studentSearch.TrackType) &&
               (studentSearch.courseId == null || s.StudentCourses.Any(sc => sc.CourseId == studentSearch.courseId)) &&

               (String.IsNullOrEmpty(studentSearch.Name) ||
                (!String.IsNullOrEmpty(s.User.FirstName) && s.User.FirstName.ToLower().Trim().Contains(studentSearch.Name)) ||
                (!String.IsNullOrEmpty(s.User.LastName) && s.User.LastName.ToLower().Trim().Contains(studentSearch.Name)))

                ),
            "User,DepartmentBranch,DepartmentBranch.Department,DepartmentBranch.Branch,DepartmentBranch.InstructorCourses,StudentCourses.Course").GroupBy(s => s.Id).Select(s => s.First()).Where(s => s.User.Status == (int)Status.Active).ToList();
                studentsMV = _mapper.Map<List<StudentMV>>(data);
                int TotalCounts = studentsMV.Count();
                if (TotalCounts > 0)
                {
                    studentsMV = studentsMV.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
                PaginatedData<StudentMV> paginatedData = new PaginatedData<StudentMV>
                {
                    Items = studentsMV,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "error occuired while Fetching Students by instructor ID ");
                return null;

            }

        }



        public int Add(StudentMV student)
        {
            int result = 0;
            try
            {
                var userExist = _unitOfWork.UserRepo.FirstOrDefault(u => u.Email == student.User.Email);
                var newStudent = new Student();
                if (userExist == null)
                {
                    var departmentBranchId = _unitOfWork.DepartmentBranchRepo.GetAll(d => d.DepartmentId == student.DepartmentId && d.BranchId == student.BranchId).FirstOrDefault()?.Id;
                    if (departmentBranchId == null)
                    {

                        throw new Exception("Department Branch not found");

                    }
                    student.DepartmentBranchId = (int)departmentBranchId;
                    newStudent = _mapper.Map<Student>(student);
                    newStudent.User.CreatedAt = DateTime.Now;
                    newStudent.User.Password = PasswordHelper.HashPassword("123456");
                    newStudent.User.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    newStudent.User.Status = (int)Status.Active;
                    newStudent.User.UserTypes.Add(_unitOfWork.UserTypeRepo.FirstOrDefault(s => s.TypeName == Constants.UserTypes.Student));
                    _unitOfWork.StudentRepo.Insert(newStudent);
                    result = _unitOfWork.Save();

                }
                else
                {
                    result = -1;
                }
                if (result > 0)
                {

                    var studentCourses = _unitOfWork.StudentRepo.AddStudentCoursesAccordingToDepartmentId((int)newStudent.Id, student.DepartmentId);
                    if (studentCourses > 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = -2;
                    }


                }

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "error occuired while adding new student ");
                return 0;
            }
        }
        public PaginatedData<StudentMV> GetAllPaginated(StudentSearchMV studentSearch, int PageSize = 10, int Page = 1)
        {
            try
            {
                List<StudentMV> studentMVs = new List<StudentMV>();
                List<Student> data = _unitOfWork.StudentRepo.GetAll(
                           s => (studentSearch.TrackType == null || s.TrackType == studentSearch.TrackType) &&
                           (studentSearch.BranchId == null || s.DepartmentBranch.BranchId == studentSearch.BranchId) &&
                           (studentSearch.Status != (int)Status.Deleted ? s.User.Status != (int)Status.Deleted : s.User.Status == (int)Status.Deleted) &&
                           (studentSearch.Status == null || s.User.Status == studentSearch.Status) &&
                           (studentSearch.DepartmentId == null || studentSearch.DepartmentId == s.DepartmentBranch.DepartmentId) &&
                           (String.IsNullOrEmpty(studentSearch.Name) ||
                           (!String.IsNullOrEmpty(s.User.FirstName) && s.User.FirstName.ToLower().Trim().Contains(studentSearch.Name)) ||
                           (!String.IsNullOrEmpty(s.User.LastName) && s.User.LastName.ToLower().Trim().Contains(studentSearch.Name)))


                    , "User,DepartmentBranch.Department,DepartmentBranch.Branch").OrderByDescending(s => s.User.CreatedAt).ToList();
                studentMVs = _mapper.Map<List<StudentMV>>(data);
                int TotalCounts = studentMVs.Count();
                if (TotalCounts > 0)
                {
                    studentMVs = studentMVs.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
                PaginatedData<StudentMV> paginatedData = new PaginatedData<StudentMV>
                {
                    Items = studentMVs,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving student data ");
                return null;

            }
        }

        public StudentMV GetById(int id)
        {
            try
            {
                StudentMV studentMV = new StudentMV();
                var student = _unitOfWork.StudentRepo.FirstOrDefault(s => s.Id == id, "User,DepartmentBranch.Department,DepartmentBranch.Branch");
                if (student == null)
                {
                    return studentMV;
                }
                studentMV = _mapper.Map<StudentMV>(student);
                return studentMV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving student data ");
                return null;
            }
        }
        public StudentMV GetStudentCoursesWithInstructor(int Student_Id, int Instructor_ID)
        {
            try
            {


                StudentMV studentMV = new StudentMV();
                var student = _unitOfWork.StudentRepo.FirstOrDefault(s => s.Id == Student_Id &&
                 s.DepartmentBranch.InstructorCourses.Any(ic => ic.Instructor.UserId == Instructor_ID),
                "DepartmentBranch.InstructorCourses.Instructor,User,DepartmentBranch.InstructorCourses.Course,DepartmentBranch.Department,DepartmentBranch.Branch");

                if (student == null)
                {
                    return studentMV;
                }
                student.DepartmentBranch.InstructorCourses = student.DepartmentBranch.InstructorCourses.Where(ic => ic.Instructor.UserId == Instructor_ID).ToList();
                studentMV = _mapper.Map<StudentMV>(student);

                return studentMV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving student data ");
                return null;
            }
        }

        public int Update(StudentMV student)
        {
            int result = 0;
            try
            {
                var studentExist = _unitOfWork.StudentRepo.FirstOrDefault(s => s.Id == student.Id, "User.UserTypes,DepartmentBranch");
                if (studentExist != null)
                {
                    var emailExist = _unitOfWork.UserRepo.FirstOrDefault(u => u.Email == student.User.Email)?.Email;
                    if (emailExist != null && studentExist.User.Email != emailExist)
                    {

                        result = -1;
                    }
                    else
                    {
                        if (studentExist.DepartmentBranch.DepartmentId != student.DepartmentId && _unitOfWork.StudentRepo.DoesStudentFinishedAtLeastOneCourse(studentExist.Id) == 1)
                        {
                            result = -2;

                        }
                        else
                        {
                            var departmentBranch = _unitOfWork.DepartmentBranchRepo.FirstOrDefault(d => d.DepartmentId == student.DepartmentId && d.BranchId == student.BranchId);
                            if (departmentBranch == null)
                            {
                                throw new Exception("Department Branch not found");
                            }
                            student.DepartmentBranchId = (int)departmentBranch.Id;
                            student.User.Password = studentExist.User.Password;
                            student.User.CreatedAt = studentExist.User.CreatedAt;
                            student.User.CreatedBy = studentExist.User.CreatedBy;
                            student.User.Status = studentExist.User.Status;
                            _mapper.Map(student, studentExist);
                            studentExist.DepartmentBranch = departmentBranch;
                            studentExist.User.UpdatedAt = DateTime.Now;
                            studentExist.User.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                            studentExist.User.Id = studentExist.UserId;
                            studentExist.User.UserTypes.Add(_unitOfWork.UserTypeRepo.FirstOrDefault(u => u.TypeName == Constants.UserTypes.Student));
                            _unitOfWork.StudentRepo.Update(studentExist);
                            result = _unitOfWork.Save();

                        }

                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while updating student data ");
                return 0;
            }
        }

        public int ChangeStatus(int id, int status)
        {
            try
            {
                var student = _unitOfWork.StudentRepo.FirstOrDefault(s => s.Id == id, "User");
                if (student != null)
                {
                    student.User.Status = status;
                    student.User.UpdatedAt = DateTime.Now;
                    student.User.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    _unitOfWork.StudentRepo.Update(student);
                    return _unitOfWork.Save();
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while changing student status ");
                return 0;
            }
        }

        public int? GetStudentGrade(int Courseid)
        {
            try
            {
                var userIdString = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                {
                    _logger.LogWarning("UserId is not found or invalid in the HttpContext.");
                    return 0;
                }
                var studentId = _unitOfWork.StudentRepo.FirstOrDefault(s => s.UserId == userId).Id;
                var studentgrade = _unitOfWork.StudentCourseRepo.GetAll(s => s.CourseId == Courseid && s.StudentId == studentId).Select(c => c.FinalGradePercent).FirstOrDefault();
                return studentgrade;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while geting course grade ");
                return 0;
            }
        }

    }

}
