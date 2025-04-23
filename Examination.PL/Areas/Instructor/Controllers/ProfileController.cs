using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Instructor.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
