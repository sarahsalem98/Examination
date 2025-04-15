using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly ICourseService _courseService;
        public ExamController(IExamService examService, ICourseService courseService)
        {
            _examService = examService;
            _courseService = courseService;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Exams";
            ViewBag.Courses = _courseService.GetCourseByStatus((int)Status.Active);
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();

            return View();
        }
        public IActionResult List(ExamSearchMV examSearch,int Page = 1, int PageSize = 10)
        {
            ViewData["Title"] = "Exams List";
            var exams = _examService.GetAllPaginated(examSearch,PageSize,Page);
            return View(exams);
        }
    }
}
