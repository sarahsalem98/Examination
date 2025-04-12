using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InstructorController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
