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
        private readonly IExamService _examService;
        public ExamQuestionController(IExamQuestionService examQuestionService, ICourseService courseService, IExamService examService)
        {
            _examQuestionService = examQuestionService;
            _courseService = courseService;
            _examService = examService;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Exam Questions";
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.Courses = _courseService.GetCourseByStatus((int)Status.Active);
            return View();
        }

        public IActionResult List(ExamQuestionSearchMV searchMV, int PageSize = 7, int Page = 1)
        {

            ViewData["Title"] = "Exam Questions List";
            var exams = _examQuestionService.GetAllExamsQuestions(searchMV, PageSize, Page);
            return View(exams);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewData["Title"] = "Exam Question Details";
            var Exams = _examService.GetByStatus();
            ViewBag.Exams = Exams;
            var examQuestionModel = new ExamQuestionMV();
            if (id > 0)
            {
                examQuestionModel = _examQuestionService.GetById(id);
                // return View(examQuestion);
            }
            return View(examQuestionModel);
        }

        [HttpPost]
        public IActionResult Details(ExamQuestionMV model)
        {
            ResponseMV responseMV = new ResponseMV();
            if (model.Id == 0)
            {
                var result = _examQuestionService.Add(model);
                if (result > 0)
                {
                    responseMV.Success = true;
                    responseMV.Message = "Exam Question Added Successfully";
                    responseMV.RedirectUrl = Url.Action("Index", "ExamQuestion", new { area = "Admin" });

                }
                else
                {
                    responseMV.Success = false;
                    responseMV.Message = "something went wrong";
                    responseMV.RedirectUrl = null;

                }
            }
            else
            {
                var result = _examQuestionService.Update(model);
                if (result > 0)
                {
                    responseMV.Success = true;
                    responseMV.Message = "Exam Question updated Successfully";
                    responseMV.RedirectUrl = Url.Action("Index", "ExamQuestion", new { area = "Admin" });
                }
                else
                {
                    responseMV.Success = false;
                    responseMV.Message = "something went wrong";
                    responseMV.RedirectUrl = null;

                }
            }
            return Json(responseMV);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, int status)
        {
            ResponseMV responseMV = new ResponseMV();
            if (id > 0)
            {
                var result = _examQuestionService.ChangeStatus(id, status);
                if (result > 0)
                {
                    responseMV.Success = true;
                    responseMV.Message = "Exam Question Status Changed Successfully";
                }
            }
            else
            {
                responseMV.Success = false;
                responseMV.Message = "Exam Question not found";
            }
            return Json(responseMV);
        }
    }
}
