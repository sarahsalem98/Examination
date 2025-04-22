using Examination.PL.Attributes;
using Examination.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.ModelViews;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService _courseService)
        {
            courseService = _courseService;
        }
        public IActionResult Index()
        {
            var courses = courseService.GetCoursesByStudent();
            return View(courses);
        }
    }
}
