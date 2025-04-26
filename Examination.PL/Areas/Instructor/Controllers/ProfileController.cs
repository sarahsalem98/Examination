using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class ProfileController : Controller
    {
        private readonly IInstructorService _instructorService;

        public ProfileController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var instructor = _instructorService.GetProfile(userId);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }
        [HttpPost]
        public IActionResult UpdatePassword([FromBody] InstructorPasswordUpdateMV model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation error." });

            var result = _instructorService.UpdatePassword(model);

            if (result == -1)
                return Json(new { success = false, message = "Current password is incorrect." });

            return Json(new { success = result > 0 });
        }



    }
}
