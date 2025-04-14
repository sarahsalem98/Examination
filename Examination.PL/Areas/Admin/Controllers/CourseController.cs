using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CourseController : Controller
    {

        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Courses";

            return View();
        }

        public IActionResult List(string courseSearch, int Page = 1, int PageSize = 10)
        {
            ViewData["Title"] = "Students List";
            var courses = _courseService.GetAllPaginated(courseSearch, PageSize, Page);
            return View(courses);
        }


    }
}
