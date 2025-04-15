using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly IUnitOfWork _unitOfWork;


        public DepartmentController(IDepartmentService departmentService, IBranchService branchService, IUnitOfWork unitOfWork)
        {
            _departmentService = departmentService;
            _branchService = branchService;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Departments";
            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>()
                                   .Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();

            var departments = _departmentService.GetAllPaginated(new DepartmentSearchMV(), 10, 1);
            return View(departments);
        }




        public IActionResult List(DepartmentSearchMV departmentSearch, int Page = 1, int PageSize = 10)
        {
            var departments = _departmentService.GetAllPaginated(departmentSearch, PageSize, Page);
            return View(departments);
        }



        [HttpGet]
        public IActionResult AddUpdate(int id)
        {
            ViewData["Title"] = "Add / Update Department";
            var department = new DepartmentMV();

            if (id > 0)
            {
                department = _departmentService.GetById(id);
                if (department == null)
                    return NotFound();

                // Preload selected branch IDs for editing
                ViewBag.SelectedBranchIds = _unitOfWork.DepartmentBranchRepo
                    .GetAll(b => b.DepartmentId == id)
                    .Select(b => b.BranchId)
                    .ToList();
            }
            else
            {
                ViewBag.SelectedBranchIds = new List<int>();
            }

            ViewBag.Branches = _branchService.GetByStatus((int)Status.Active);
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>()
                .Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();

            return View(department);
        }

        [HttpPost]
        public IActionResult AddUpdate(DepartmentMV model)
        {
            ResponseMV response = new();

            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var result = _departmentService.Update(model);
                    response.Success = result > 0;
                    response.Message = result > 0 ? "Department updated successfully" : "Error occurred while updating department";
                }
                else
                {
                    var result = _departmentService.Add(model);
                    response.Success = result > 0;
                    response.Message = result > 0 ? "Department added successfully" : "Error occurred while adding department";
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
