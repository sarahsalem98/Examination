using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;
using Microsoft.AspNetCore.Authorization;
using Examination.PL.ModelViews;

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


        //  [HttpPost]
        //public IActionResult UpdateProfile([FromBody] StudentMV student)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });

        //    var result = _studentService.UpdateProfile(student);

        //    if (result <= 0)
        //        return Json(new { success = false, message = "Update failed inside service" });

        //    return Json(new { success = true });
        //}
        //        [HttpPost]
        //public IActionResult UpdateProfile(StudentMV student)
        //{
        //    if (!ModelState.IsValid)
        //        return Json(new { success = false, message = "Invalid data" });

        //    var result = _studentService.UpdateProfile(student);
        //    return Json(new { success = result > 0 });
        //}

        //[HttpPost]
        //public IActionResult UpdateProfile([FromBody] StudentMV student)
        //{
        //    if (!ModelState.IsValid)
        //        return Json(new { success = false, message = "Invalid data" });

        //    var result = _studentService.UpdateProfile(student);

        //    return Json(new { success = result > 0 });
        //}
        [HttpPost]
        public IActionResult UpdateProfile([FromBody] StudentUpdateProfileMV student)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            var result = _studentService.UpdateProfile(student);
            return Json(new { success = result > 0 });
        }

    }
}
