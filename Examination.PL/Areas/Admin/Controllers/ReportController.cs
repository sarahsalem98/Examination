using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetReport(string ReportName)
        {
            ViewBag.ReportName= ReportName;
          return  View();
        }
    }
}
