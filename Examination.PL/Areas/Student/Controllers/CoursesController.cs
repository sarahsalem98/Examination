using Examination.PL.Attributes;
using Examination.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.ModelViews;
using Examination.PL.BL;
using Examination.PL.General;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        readonly ITopicService topicService;

        public CoursesController(ICourseService _courseService, ITopicService _topicService)
        {
            courseService = _courseService;
            topicService = _topicService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List(string name, int page = 1, int pagesize = 8)
        {
            var courses = courseService.GetCoursesByStudent(name, pagesize, page);
            return View(courses);
        }

        public IActionResult Details(int Courseid)
        {
            var topics = topicService.GetTopicsByCourseId(Courseid);
            ViewBag.Course = courseService.GetCourseByID(Courseid);
            return View(topics);
        }
    }
}
