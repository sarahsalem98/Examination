using Azure;
using Examination.PL.Attributes;
using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
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
        public IActionResult AddUpdate(int id)
        {

            ViewBag.Courses = _courseService.GetCourseByStatus((int)Status.Active);
            ExamMV exam = new ExamMV();
            if (id > 0)
            {
                exam = _examService.GetById(id);
                if (exam == null)
                {
                    return NotFound();
                }

            }
            return View(exam);
        }
        [HttpPost]
        public IActionResult AddUpdate(ExamMV exam)
        {
            ResponseMV response = new ResponseMV();
            if (ModelState.IsValid)
            {
                if (exam.Id > 0)
                {
                    var res = _examService.Update(exam);
                    if (res == 1)
                    {
                        response.Success = true;
                        response.Message = "Exam Updated Successfully";
                        response.Data = exam;
                    }
                    else if(res==-1)
                    {
                        response.Success = false;
                        response.Message = "Exam With This Type And This Course Already Exsist";
                        response.Data = exam;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Exam Updated Failed";
                        response.Data = exam;
                    }
                }
                else
                {
                    var res = _examService.Add(exam);
                    if (res == 1)
                    {
                        response.Success = true;
                        response.Message = "Exam Added Successfully";
                        response.Data = exam;
                    }
                    else if (res == -1)
                    {
                        response.Success = false;
                        response.Message = "Exam With This Type And This Course Already Exsist";
                        response.Data = exam;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Exam Added Failed";
                        response.Data = exam;
                    }
                }
            }else
            {
                response.Success = false;
                response.Message = "Please Enter Valid Data";
                response.Data = exam;
            }
            return Json(response);
        }
        public IActionResult ChangeStatus(int id,int status)
        {
            ResponseMV response = new ResponseMV();
            var exam=_examService.GetById(id);
            if(exam==null)
            {
                return NotFound();
            }else
            {
                var res=_examService.ChnageStatus(id,status);
                if(res == 1)
                {
                    response.Success = true;
                    response.Message = "Status Changed Successfully";
                   
                }
                else
                {
                    response.Success = false;
                    response.Message = "Status Changed Failed";
                   
                }

            }
            return Json(response);  
        }
    }
}
