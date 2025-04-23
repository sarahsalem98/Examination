using Examination.PL.Attributes;
using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;

namespace Examination.PL.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [UserTypeAuthorize(Constants.UserTypes.Instructor)]
    public class GeneratedExamController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly ICourseService _courseService;
        private readonly IInstructorCourseService _instructorCourseService;
        private readonly IGeneratedExamService _generatedExamService;
        private readonly IExamService _examService;
        public GeneratedExamController(IStudentService studentService, IDepartmentService departmentService, IBranchService branchService, ICourseService courseService, IInstructorCourseService instructorCourseService,IGeneratedExamService generatedExamService,IExamService examService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _branchService = branchService;
            _courseService = courseService;
            _instructorCourseService = instructorCourseService;
            _generatedExamService = generatedExamService;
            _examService = examService;

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
        [HttpGet]
        public IActionResult GenerateExam()
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.Exams=_examService.GetByInstructor(Loggedinuser);
            
          
            return View();
        }
       [HttpPost]
        public IActionResult GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ, int CreatedBy, DateOnly TakenDate, TimeOnly takenTime)
        {

            ResponseMV response = new ResponseMV();
            var res = _generatedExamService.GenerateExam(ExamId, DepartmentId, BranchId, NumsTS, NumsMCQ, 11, TakenDate, takenTime);
            if(res>0)
            {
                response.Success = true;
                response.Message = "Exam Generated Successfully";

            }
            else if (res < 0)
            {
                response.Success = false;
                response.Message = "Exam Generated Failed";

            }
            return Json(response);
        }
        public IActionResult GetDepartmentsByBranchId(int BranchId)
        {
            ResponseMV response = new ResponseMV();
            var departments = new List<DepartmentMV>();
            if (BranchId > 0)
            {
                departments = _departmentService.GetByBranch(BranchId);
                if (departments == null || departments.Count() == 0)
                {
                    response.Success = false;
                    response.Message = "No departments found for this branch";
                    response.Data = null;
                }
                else
                {

                    response.Success = true;
                    response.Message = "All Departments";
                    response.Data = departments;
                }


            }
            else
            {
                response.Success = false;
                response.Message = "some thing went wrong";
                response.Data = null;
            }
            return Json(response);
        }
    }
}
