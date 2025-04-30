using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Examination.DAL.Entities;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class PreviousExamController : Controller
    {
        private readonly IGeneratedExamService generatedExamService;

        public PreviousExamController(IGeneratedExamService _generatedExamService)
        {
            generatedExamService = _generatedExamService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List(GeneratedExamSearchMV search, int page = 1, int pagesize = 8)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var PreviousExam = generatedExamService.GetPreviousExams(userId.ToString(), search, pagesize, page);
            ViewBag.Grades = HttpContext.Items["ExamGrades"];
            return View(PreviousExam);
        }
    }
}
