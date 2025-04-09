using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class ExamController : Controller
    {
        public IActionResult Previous()
        {
            return View();
        }
        public IActionResult PreviousList()
        {
            return View();
        }

    }
}
