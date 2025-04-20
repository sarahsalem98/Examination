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
            ViewBag.Status = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.courses = _courseService.GetCourseByInstructor(Loggedinuser);
            return View();
        }
        public IActionResult List(CourseSearchMV courseSearch, int Page = 1, int PageSize = 10)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            var courses=_instructorCourseService.GetCourseByInstructorPaginated(Loggedinuser, courseSearch,Page,PageSize);

            return View(courses);
        }
    }
}
