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
 
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly IGeneratedExamService _generatedExamService;
        private readonly IExamService _examService;
        private readonly IInstructorService _instructorService;
        private readonly ICourseService _courseService;
        public GeneratedExamController( IDepartmentService departmentService, IBranchService branchService, ICourseService courseService, IGeneratedExamService generatedExamService,IExamService examService, IInstructorService instructorService)
        {
            _departmentService = departmentService;
            _branchService = branchService;
            _generatedExamService = generatedExamService;
            _examService = examService;
            _instructorService = instructorService;
            _courseService = courseService;

        }
        public IActionResult Index()
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.courses = _courseService.GetCourseByInstructor(Loggedinuser);

            return View();
        }
        public IActionResult List(GeneratedExamSearchMV search, int PageSize = 10, int Page = 1)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            var exams =_generatedExamService.GetAllPaginated(Loggedinuser,search,PageSize,Page);
            return View(exams);

        }
        [HttpGet]
        public IActionResult GenerateExam(int id)
        {
            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            ViewBag.Branches = _branchService.GetBranchesByInstructor(Loggedinuser);

            GeneratedExamMV exam=new GeneratedExamMV();
            if (id > 0)
            {
                 exam = _generatedExamService.GetByID(id);
                if (exam == null)
                {
                    return NotFound();
                }
            }
          
        
            
          
            return View(exam);
        }
       [HttpPost]
        public IActionResult GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ,  DateOnly TakenDate, TimeOnly takenTime)
        {

            var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
            var Instructor_id=_instructorService.GetInstructorIdbyUserID(Loggedinuser);
            ResponseMV response = new ResponseMV();
            var res = _generatedExamService.GenerateExam(ExamId, DepartmentId, BranchId, NumsTS, NumsMCQ,Instructor_id, TakenDate, takenTime);
            if(res==1)
            {
                response.Success = true;
                response.Message = "Exam Generated Successfully";

            }
            else if (res == 0)
            {
                response.Success = false;
                response.Message = "Exam Generated Failed";

            }
            else if (res== -1)
            {
                response.Success = false;
                response.Message = "Course Doesn't Finished Yet";

            }
            else if(res==-2)
            {
                response.Success = false;
                response.Message = "You already Generate Exam Before";
            }
            return Json(response);
        }
        public IActionResult GetDepartmentsByBranchId(int BranchId)
        {
            ResponseMV response = new ResponseMV();
            var departments = new List<DepartmentMV>();
            if (BranchId > 0)
            {
                var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
                departments = _departmentService.GetByBranchAndInstructor(BranchId,Loggedinuser);
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
        public IActionResult GetExamsByInstructorDepartmentBranch(int department_id, int branch_id)
        {
            ResponseMV response = new ResponseMV();
            var exams = new List<ExamMV>();
           
                var Loggedinuser = int.Parse(User.FindFirst("UserId")?.Value);
                exams = _examService.GetByInstructorDepartmentBranch(Loggedinuser,department_id,branch_id );
                if (exams == null || exams.Count() == 0)
                {
                    response.Success = false;
                    response.Message = "No exams found for this departmentbranch";
                    response.Data = null;
                }
                else
                {

                    response.Success = true;
                    response.Message = "All exams";
                    response.Data = exams;
                }


            
            
            return Json(response);
        }
    }
}
