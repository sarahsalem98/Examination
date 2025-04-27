using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class CommingExamController : Controller
    {
        private readonly IGeneratedExamService GeneratedExamService;

        public CommingExamController(IGeneratedExamService _GeneratedExamService)
        {
            GeneratedExamService = _GeneratedExamService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var commingExams = new List<GeneratedExamMV>();
            commingExams = GeneratedExamService.CommingExam(userId.ToString());
            return View(commingExams);
        }
    }
}
