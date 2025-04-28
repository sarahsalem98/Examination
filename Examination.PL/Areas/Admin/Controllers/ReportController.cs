using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class ReportController : Controller
    {
        public IActionResult GetTopics()
        {
            return View();
        }
    }
}
