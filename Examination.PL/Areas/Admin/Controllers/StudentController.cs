using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        public StudentController(IStudentService studentService, IDepartmentService departmentService, IBranchService branchService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _branchService = branchService;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Students";
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.TrackTypes = Enum.GetValues(typeof(TrackType)).Cast<TrackType>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View();
        }
        public IActionResult List(StudentSearchMV studentSearch, int Page = 1, int PageSize = 10)
        {
            ViewData["Title"] = "Students List";
            var students = _studentService.GetAllPaginated(studentSearch, PageSize, Page);
            return View(students);
        }

        [HttpGet]
        public IActionResult AddUpdate(int id)
        {
            ViewData["Title"] = "Add Update Student";
            var student = new StudentMV();
            if (id > 0)
            {
                student = _studentService.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
            }
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.TrackTypes = Enum.GetValues(typeof(TrackType)).Cast<TrackType>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View(student);

        }

        [HttpPost]
        public IActionResult AddUpdate(StudentMV model)
        {
            if (ModelState.IsValid) { 
             
                if(model.Id > 0)
                {
                    // Update logic here
                }
                else
                {
                    var result = _studentService.Add(model);
                    if (result > 0)
                    {
                        TempData["Success"] = "Student added successfully.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        TempData["Error"] = "Failed to add student.";
                    }
                }
            }
            else
            {
                TempData["Error"] = "Invalid data.";


            }


        }

    }
}
