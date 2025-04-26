using AspNetCoreGeneratedDocument;
using Examination.PL.Attributes;
using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [UserTypeAuthorize(Constants.UserTypes.Instructor)]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly ICourseService _courseService;
        private readonly IGeneratedExamQService _generatedExamQService;
       
        public StudentController(IStudentService studentService, IDepartmentService departmentService, IBranchService branchService, ICourseService courseService, IGeneratedExamQService generatedExamQService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _branchService = branchService;
            _courseService = courseService;
            _generatedExamQService = generatedExamQService;
        }
        public IActionResult Index()
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ViewData["Title"] = "Students";
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.TrackTypes = Enum.GetValues(typeof(TrackType)).Cast<TrackType>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.courses = _courseService.GetCourseByInstructor(Loggedinuser);


            return View();
        }
        public IActionResult List(StudentSearchMV studentSearch, int Page = 1, int PageSize = 10)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ViewData["Title"] = "Students List";
            var students = _studentService.GetStudentsByInstructorPaginated(Loggedinuser, studentSearch, PageSize, Page);
           
          
           return View(students);
        }
        public IActionResult StudentDetails(int id)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);

            var student =_studentService.GetStudentCoursesWithInstructor(id, Loggedinuser);
            if (student == null)
            {
                return NotFound();
            }else
            {
                return View(student);
            }
            
        }
        public IActionResult ShowExam(int Student_id, int GeneretaedExam_id)
        {
            ViewBag.Studentid = Student_id;
            var questions = new List< GeneratedExamQMV>();
            questions=_generatedExamQService.GetGeneratedExam( GeneretaedExam_id);

            return View(questions);
        }
        
    }
}
