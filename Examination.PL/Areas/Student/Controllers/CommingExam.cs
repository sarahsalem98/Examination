using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class CommingExam : Controller
    {
        private readonly IGeneratedExamService GeneratedExamService;

        public CommingExam(IGeneratedExamService _GeneratedExamService)
        {
            GeneratedExamService = _GeneratedExamService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var commingExams = GeneratedExamService.CommingExam(userId.ToString());
            return View(commingExams);
        }
    }
}
