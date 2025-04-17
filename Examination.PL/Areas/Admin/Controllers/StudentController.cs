using Examination.PL.Attributes;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        public StudentController(IStudentService studentService, IDepartmentService departmentService, IBranchService branchService)
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _branchService = branchService;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Students";
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.TrackTypes = Enum.GetValues(typeof(TrackType)).Cast<TrackType>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View();
        }
        public IActionResult List(StudentSearchMV studentSearch, int Page = 1, int PageSize = 10)
        {
            ViewData["Title"] = "Students List";
            var students = _studentService.GetAllPaginated(studentSearch, PageSize, Page);
            return View(students);
        }

        [HttpGet]
        public IActionResult AddUpdate(int id)
        {
            ViewData["Title"] = "Add Update Student";
            var student = new StudentMV();
            if (id > 0)
            {
                student = _studentService.GetById(id);
                student.DepartmentId = student.DepartmentBranch.DepartmentId;
                student.BranchId = student.DepartmentBranch.BranchId;
                if (student == null)
                {
                    return NotFound();
                }
            }
            ViewBag.Departments = _departmentService.GetByStatus((int)Status.Active);
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.TrackTypes = Enum.GetValues(typeof(TrackType)).Cast<TrackType>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View(student);

        }

        [HttpPost]
        public IActionResult AddUpdate(StudentMV model)
            {

            ResponseMV response = new ResponseMV();
            if (ModelState.IsValid)
            {

                if (model.Id > 0)
                {
                    var result = _studentService.Update(model);
                    if(result > 0)
                    {
                        response.Success = true;
                        response.Message = "Student updated successfully";
                        response.RedirectUrl = null;
                    }
                    else if (result == -1)
                    {
                        response.Success = false;
                        response.Message = "Email is already exist";
                    }else if (result == -2)
                    {
                        response.Success = false;
                        response.Message = "can not change department while already courses has been taken";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error occurred while updating student";
                    }
                }
                else
                {
                    var result = _studentService.Add(model);
                    if (result > 0)
                    {
                        response.Success = true;
                        response.Message = "Student added successfully";
                        response.RedirectUrl = null;
                    }else if (result == -1)
                    {
                        response.Success = false;
                        response.Message = "Email is alraedy exist";
                    }else if(result == -2)
                    {
                        response.Success = false;
                        response.Message = "something went wrong while adding studentcourses related to department";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error occurred while adding student";

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

        [HttpPost]
        public IActionResult ChangeStatus(int id , int status)
        {
            ResponseMV response = new ResponseMV();
            if (id > 0)
            {
                var result = _studentService.ChangeStatus(id, status);
                if (result > 0)
                {
                    response.Success = true;
                    response.Message = "Student status changed successfully";
                    response.RedirectUrl = null;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error occurred while changing student status";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid student id";
            }
            return Json(response);


        }

        [HttpGet]
        public IActionResult GetDepartmentsByBranchId(int BranchId)
        {
            ResponseMV response = new ResponseMV(); 
            var departments = new List<DepartmentMV>();
            if (BranchId > 0)
            {
                departments = _departmentService.GetByBranch(BranchId);
                if (departments == null || departments.Count() == 0)
                {
                    response.Success = false;
                    response.Message = "No departments found for this branch";
                    response.Data = null;
                }
                else
                {

                    response.Success = true;
                    response.Message = "Departments found";
                    response.Data = departments;
                }


            }
            else
            {
                response.Success = false;
                response.Message = "Invalid branch id";
                response.Data = null;
            }
            return Json(response);
        }

    }
}
