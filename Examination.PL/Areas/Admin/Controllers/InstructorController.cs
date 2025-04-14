using Azure;
using Examination.DAL.Repos;
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
    public class InstructorController : Controller
    {
        private readonly IInstructorService InstructorService;
        private readonly IDepartmentService departmentService;
        private readonly IBranchService branchService;
        private readonly ICourseService courseService;
        public InstructorController(IInstructorService _InstructorService, IDepartmentService _departmentService, IBranchService _branchService, ICourseService _courseService)
        {
            InstructorService = _InstructorService;
            branchService = _branchService;
            departmentService = _departmentService;
            courseService = _courseService;
        }
        public IActionResult Index()
        {
            ViewBag.Departments = departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = branchService.GetByStatus((int)Status.Active);
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View();
        }
        public IActionResult List(InstructorSearchMV InstructorSearch, int page = 1, int pagesize = 10)
        {

            var instructors=InstructorService.GetAllPaginated(InstructorSearch, page, pagesize);
            return View(instructors);
        }
        [HttpGet]
        public IActionResult AddUpdate(int id)
        {
            ViewData["Title"] = "Add Update Student";
            ViewBag.branches = branchService.GetByStatus((int)Status.Active);
            var instructor = new InstructorMV();
            if(id>0)
            {
                instructor = InstructorService.getById(id);
                if (instructor == null)
                {
                    return NotFound();
                }
            }
            return View(instructor);
        }
        [HttpPost]
        public IActionResult AddUpdate(InstructorMV instructor)
        {
           ResponseMV response = new ResponseMV();
            if (ModelState.IsValid)
            {
                if(instructor.Id>0)
                {
                    var res=InstructorService.Update(instructor);
                    if (res > 0)
                    {
                        response.Success = true;
                        response.Message = "Instructor Updated successfully";
                        response.Data = instructor;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "something wont wrong";
                    }
                }
                else
                {
                   var res= InstructorService.Add(instructor);
                    if(res>0)
                    {
                        response.Success = true;
                        response.Message = "Instructor Added successfully";
                        response.Data = instructor;
                    }
                    else if (res==-1)
                    {
                        response.Success = false;
                        response.Message = "Instructor Already Exsists";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "something wont wrong";
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

               
                response.Success = false;
                response.Message = "Invalid Data";
            }

            return Json(response);
        }
        [HttpPut]
        public IActionResult ChangeStatus(int id,int status)
        {
            ResponseMV response = new ResponseMV();
        
           var res=InstructorService.ChangeStatus(id, status);
            if(res==0)
            {
                response.Success = false;
                response.Message = "Something Went Wrong";
            }
            else  if (res == -1)
                {
                    response.Success = false;
                    response.Message = "instructor Not found";
                }
            else
            {
                response.Success = true;
                response.Message = $"instructor Status Changed Successfully";
              
            }

            return Json(response);
        }
        public IActionResult GetCoursesByDepartmenID(int DepartmentId)
        {
            ResponseMV response = new ResponseMV();
            var courses=new List<CourseMV>();
            if (DepartmentId > 0)
            {
                courses=courseService.GetCoursesByDeaprtment(DepartmentId);
                if(courses==null)
                {
                    response.Success = false;
                    response.Message = "no courses found";
                    response.Data = null;
                }else
                {
                    response.Success = true;
                    response.Message = "AllCourses";
                    response.Data = courses;
                }

            }
            else
            {
                response.Success = false;
                response.Message = "some thing went wrong";
                response.Data = null;
            }
            return Json(response);
        }
        public IActionResult GetDepartmentsByBranchId(int BranchId)
        {
            ResponseMV response = new ResponseMV();
            var departments = new List<DepartmentMV>();
            if (BranchId > 0)
            {
                departments = departmentService.GetByBranch(BranchId);
                if (departments == null || departments.Count() == 0)
                {
                    response.Success = false;
                    response.Message = "No departments found for this branch";
                    response.Data = null;
                }
                else
                {

                    response.Success = true;
                    response.Message = "All Departments";
                    response.Data = departments;
                }


            }
            else
            {
                response.Success = false;
                response.Message = "some thing went wrong";
                response.Data = null;
            }
            return Json(response);
        }
    }
}
