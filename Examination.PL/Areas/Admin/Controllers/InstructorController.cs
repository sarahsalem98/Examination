using Azure;
using Examination.DAL.Repos;
using Examination.PL.BL;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService InstructorService;
        private readonly IDepartmentService departmentService;
        private readonly IBranchService branchService;
        public InstructorController(IInstructorService _InstructorService, IDepartmentService _departmentService, IBranchService _branchService)
        {
            InstructorService = _InstructorService;
            branchService = _branchService;
            departmentService = _departmentService;
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
       
    }
}
