using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;
using Examination.PL.General;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;




namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class BranchController : Controller
    {
        private readonly IBranchService branchService;
        private readonly IDepartmentService departmentService;

        public BranchController(IBranchService _branchService, IDepartmentService _departmentService)
        {
            branchService = _branchService;
            departmentService = _departmentService;
        }
        public IActionResult Index()
        {
            ViewBag.Locations = branchService.GetDistinctBranchLocations();
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View();
        }

        public IActionResult List(BranchSearchMV branchSearch, int page = 1, int pagesize = 10)
        {
            var branches = branchService.GetAllPaginated(branchSearch, pagesize, page);
            return View(branches);
        }
    }
}
