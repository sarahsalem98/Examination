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
        private readonly IExamService _examService;
        public ExamController(IGeneratedExamService generatedExamService, IExamService examService)
        {
            _generatedExamService = generatedExamService;
            _examService = examService;
        }
        public IActionResult Index(int GeneratedExamId)
        {

            OnGoingExamMV examGoing = new OnGoingExamMV();
            GeneratedExamId = 18;
            var exam = _generatedExamService.GetByID(GeneratedExamId);
            if (exam.TakenDate != DateOnly.FromDateTime(DateTime.Now)) {

                return NotFound();
            }
            else
            {
                var examQuestions = exam.GeneratedExamQs.OrderBy(q => q.QsOrder).Select(q => new OnGoingExamQuestion
                {
                    QId = q.Id,
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
                examGoing.GeneratedExamId = GeneratedExamId;
                examGoing.InstructorCourseId = exam.InstructorCourseId??0;
                examGoing.ExamType = exam.Exam.Type.ToLower();

                ViewBag.Exam = examGoing;
                return View();

            }
             
        }

        public IActionResult SubmitAnswer(int GenratedExamId, int QId , string StdAnswer)
        {
            int loggedUserId =int.Parse(User.FindFirst("UserId").Value);
            var res = _examService.SubmitQuestionAnswer(loggedUserId, QId, GenratedExamId, StdAnswer);
            if (res > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
           

        }
        public IActionResult CorrectExam(int GeneratedExamId , string ExamType, int InstructorCourseId)
        {

            int loggedUserId = int.Parse(User.FindFirst("UserId").Value);
            var res = _examService.CorrectExam(GeneratedExamId, loggedUserId,ExamType , InstructorCourseId, 50);
            if (res > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }

      
    }
}
