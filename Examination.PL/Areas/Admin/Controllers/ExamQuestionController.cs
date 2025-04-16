using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class ExamQuestionController : Controller
    {
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult AddUpdate()
        {
            return PartialView();
        }
    }
}
