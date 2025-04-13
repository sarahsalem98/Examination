using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;

        public DepartmentController(IDepartmentService departmentService, IBranchService branchService)
        {
            _departmentService = departmentService;
            _branchService = branchService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Departments";
            ViewBag.Statuses = Enum.GetValues(typeof(Status))
                .Cast<Status>().
                Select(e => new { Id = (int)e, Name = e.ToString() })
                .ToList();
         
            return View();
        }

        public IActionResult List(DepartmentSearchMV departmentSearch, int Page = 1, int PageSize = 10)
        {
            ViewData["Title"] = "Departments List";
            var departments = _departmentService.GetAllPaginated(departmentSearch, PageSize, Page);
            return View(departments);
        }

    }
}
