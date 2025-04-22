using Azure;
using Examination.PL.Attributes;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [UserTypeAuthorize(Constants.UserTypes.Instructor)]
    public class CourseController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly ICourseService _courseService;
        private readonly IInstructorCourseService _instructorCourseService;  
        public CourseController(IStudentService studentService, IDepartmentService departmentService, IBranchService branchService, ICourseService courseService, IInstructorCourseService instructorCourseService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _branchService = branchService;
            _courseService = courseService;
            _instructorCourseService = instructorCourseService;
        }
        public IActionResult Index()
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.Status = Enum.GetValues(typeof(CourseStatus)).Cast<CourseStatus>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.courses = _courseService.GetCourseByInstructor(Loggedinuser);
            return View();
        }
        public IActionResult List(CourseSearchMV courseSearch, int Page = 1, int PageSize = 10)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            var courses=_instructorCourseService.GetCourseByInstructorPaginated(Loggedinuser, courseSearch,Page,PageSize);

            return View(courses);
        }
        [HttpPost]
        public IActionResult CompleteCourse(int DepartmentBranchId,int course_id)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ResponseMV response = new ResponseMV();
            int res=_instructorCourseService.CompleteCourse(DepartmentBranchId, Loggedinuser, course_id);
            if (res == 1)
            {
                response.Success = true;
                response.Message = "Course Completed Succesfully";
            }
            else if (res == 0)
            {
                response.Success = false;
                response.Message = "Something Went Wronggg";
            }
            else if (res == -1)
            {
                response.Success = false;
                response.Message = "Can't Complete The Course Try Again";
            }
            else if (res == -2)
            {
                response.Success = false;
                response.Message = "Course Doesn't End Yet";
            }
            else if (res == -3)
            {
                response.Success = false;
                response.Message = "You Should Generate Exam ";
            }
            else if (res == -4)
            {
                response.Success = false;
                response.Message = "Student Waiting The Exam";
            }
            else if (res == -5)
            {
                response.Success = false;
                response.Message = "Students That Doesn't Pass Exam doesn't have Corrective Exam Yet";
            }
            else if (res == -6)
            {
                response.Success = false;
                response.Message = "The course Is Completed Already";
            }
            
            return Json(response);
        }
    }
}
