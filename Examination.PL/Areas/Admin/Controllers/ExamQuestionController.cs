using Examination.PL.Attributes;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class ExamQuestionController : Controller
    {
        private readonly IExamQuestionService _examQuestionService; 
        private readonly ICourseService _courseService; 
        public ExamQuestionController(IExamQuestionService examQuestionService, ICourseService courseService)
        {
            _examQuestionService = examQuestionService;
            _courseService = courseService;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Exam Questions";
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.Courses = _courseService.GetCourseByStatus((int)Status.Active); 
            return View();
        }

        public IActionResult List(ExamQuestionSearchMV searchMV ,int PageSize =7, int Page=1)
        {

            ViewData["Title"] = "Exam Questions List";
            var exams = _examQuestionService.GetAllExamsQuestions(searchMV, PageSize, Page);
            return View(exams);
        }
        [HttpPost]
        public IActionResult Details()
        {
            return PartialView();
        }
    }
}
