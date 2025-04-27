using Examination.PL.Attributes;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Student.Controllers
{
    [UserTypeAuthorize(Constants.UserTypes.Student)]
    [Area("Student")]
    public class ExamController : Controller
    {
        private readonly IGeneratedExamService _generatedExamService;
        public ExamController(IGeneratedExamService generatedExamService)
        {
            _generatedExamService = generatedExamService;
        }
        public IActionResult Index(int GeneratedExamId)
        {
            OnGoingExamMV examGoing = new OnGoingExamMV();
            GeneratedExamId = 18;
            var exam = _generatedExamService.GetByID(GeneratedExamId);
            var examQuestions = exam.GeneratedExamQs.OrderBy(q => q.QsOrder).Select(q => new OnGoingExamQuestion {
                QId = q.ExamQs.Id,
                GeneratedExamId = q.GeneratedExamId,
                Question = q.ExamQs.Question,
                Answers = q.ExamQs.Answers,
                RightAnswer = q.ExamQs.RightAnswer,
                Degree = q.ExamQs.Degree,
                Qorder = q.QsOrder,
            }).ToList();
            examGoing.ExamTitle = exam.Exam.Name;
            examGoing.StartTime = exam.TakenDate.ToDateTime(exam.TakenTime);
            examGoing.EndTime = exam.TakenDate.ToDateTime(exam.TakenTime).AddMinutes(exam.Exam.Duration);
            examGoing.Duration = exam.Exam.Duration;
            examGoing.Questions = examQuestions;
            ViewBag.Exam = examGoing;
            return View();
        }

      
    }
}
