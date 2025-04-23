using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;
using Microsoft.AspNetCore.Authorization;

namespace Examination.PL.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize] 
    public class ProfileController : Controller
    {
        private readonly IStudentService _studentService;

        public ProfileController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            //int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            //var student = _studentService.GetProfile(userId);

            //if (student == null)
            //    return NotFound();

            //return View(student);

            var userId = int.Parse(User.FindFirst("UserId").Value);
            var student = _studentService.GetProfile(userId);
            return View(student);
        }
    }
}
