using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService InstructorService;
        private readonly IDepartmentService departmentService;
        private readonly IBranchService branchService;
        public InstructorController(IInstructorService _InstructorService, IDepartmentService _departmentService, IBranchService _branchService)
        {
            InstructorService = _InstructorService;
            branchService = _branchService;
            departmentService = _departmentService;
        }
        public IActionResult Index()
        {
            ViewBag.Departments = departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = branchService.GetByStatus((int)Status.Active);
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View();
        }
        public IActionResult List(InstructorSearchMV InstructorSearch, int page = 1, int pagesize = 10)
        {

            var instructors=InstructorService.GetAllPaginated(InstructorSearch, page, pagesize);
            return View(instructors);
        }
        public IActionResult get(InstructorSearchMV InstructorSearch, int page = 1, int pagesize = 10)
        {
            var instructors = InstructorService.GetAllPaginated(InstructorSearch, page, pagesize);
            return Ok(instructors);
        }
    }
}
