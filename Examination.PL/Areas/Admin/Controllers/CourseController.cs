using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CourseController : Controller
    {

        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Courses";

            return View();
        }

        public IActionResult List(string courseSearch, int Page = 1, int PageSize = 10)
        {
            ViewData["Title"] = "Students List";
            var courses = _courseService.GetAllPaginated(courseSearch, PageSize, Page);
            return View(courses);
        }



        [HttpGet]
        public IActionResult AddUpdate(int id)
        {
            ViewData["Title"] = "Add Update Course";
            var course = new CourseMV();

            if (id > 0)
            {
                course = _courseService.GetCourseByID(id);
                if (course == null)
                {
                    return NotFound();
                }
            }

            return View(course);

        }




        [HttpPost]
        public IActionResult AddUpdate(CourseMV model)
        {

            ResponseMV response = new ResponseMV();
            if (ModelState.IsValid)
            {

                if (model.Id > 0)
                {
                    var result = _courseService.Update(model);
                    if (result > 0)
                    {
                        response.Success = true;
                        response.Message = "Course updated successfully";
                        response.RedirectUrl = null;
                    }
                    else if (result == -1)
                    {
                        response.Success = false;
                        response.Message = "Course Not Found";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error occurred while updating Course";
                    }
                }
                else
                {
                    var result = _courseService.Add(model);
                    if (result > 0)
                    {
                        response.Success = true;
                        response.Message = "Course added successfully";
                        response.RedirectUrl = null;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error occurred while adding Course";
                    }

                }
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid data";
            }
            return Json(response);

        }


    }
}
