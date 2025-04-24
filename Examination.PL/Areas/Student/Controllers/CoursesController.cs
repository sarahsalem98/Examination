using Examination.PL.Attributes;
using Examination.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.ModelViews;
using Examination.PL.BL;
using Examination.PL.General;
using Examination.DAL.Entities;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly ITopicService topicService;
        private readonly IStudentService studentService;


        public CoursesController(ICourseService _courseService, ITopicService _topicService,IStudentService _studentService)
        {
            courseService = _courseService;
            topicService = _topicService;
            studentService = _studentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List(string name, int page = 1, int pagesize = 8)
        {
            var courses = courseService.GetCoursesByStudent(name, pagesize, page);
            ViewBag.Grades=HttpContext.Items["Grades"];
            return View(courses);
        }

        public IActionResult Details(int Courseid)
        {
            var topics = topicService.GetTopicsByCourseId(Courseid);
            ViewBag.Course = courseService.GetCourseByID(Courseid);
            ViewBag.Grade=studentService.GetStudentGrade(Courseid);
            return View(topics);
        }
    }
}
