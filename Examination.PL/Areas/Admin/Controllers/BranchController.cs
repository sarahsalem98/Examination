using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;


namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        public IActionResult Index()
        {
            return View();
        }
    }
}
