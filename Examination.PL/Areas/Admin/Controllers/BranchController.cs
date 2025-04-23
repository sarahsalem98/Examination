using Examination.PL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Examination.PL.IBL;
using Examination.PL.General;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Mvc;




namespace Examination.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserTypeAuthorize(Constants.UserTypes.Admin)]
    public class BranchController : Controller
    {
        private readonly IBranchService branchService;

        public BranchController(IBranchService _branchService)
        {
            branchService = _branchService;
        }
        public IActionResult Index()
        {
            ViewBag.Locations = branchService.GetDistinctBranchLocations();
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
            return View();
        }

        public IActionResult List(BranchSearchMV branchSearch, int page = 1, int pagesize = 10)
        {
            var branches = branchService.GetAllPaginated(branchSearch, pagesize, page);
            return View(branches);
        }

        [HttpGet]
        public IActionResult AddUpdate(int id)
        {
            ViewData["Title"] = "Add Update Branch";

            var branch = new BranchMV();
            if (id > 0)
            {
                branch = branchService.GetById(id);
                if (branch == null)
                {
                    return NotFound();
                }
            }
            return View( branch);
        }

        [HttpPost]
        public IActionResult AddUpdate([FromBody] BranchMV branch)
        {
            ResponseMV response = new ResponseMV();

            if (branch.Id > 0)
            {
                var res = branchService.Update(branch);
                if (res == -1)
                {
                    response.Success = false;
                    response.Message = "branch does not exist";
                    response.Data = branch;
                }
                else if (res == 0)
                {
                    response.Success = false;
                    response.Message = "Error occurred while updating branch";
                    response.Data = branch;
                }
                else
                {
                    response.Success = true;
                    response.Message = "Branch updated successfully";
                    response.Data = branch;
                }
            }
            else
            {
                var res = branchService.Add(branch);
                if (res == -1)
                {
                    response.Success = false;
                    response.Message = "branch already exists";
                    response.Data = branch;
                }
                else if (res == 0)
                {
                    response.Success = false;
                    response.Message = "Error occurred while adding branch";
                    response.Data = branch;
                }
                else
                {
                    response.Success = true;
                    response.Message = "Branch added successfully";
                }
            }
            return Json(response);

        }

        public IActionResult ChangeStatus(int id,int status)
        {
            ResponseMV response = new ResponseMV();
            if (status == (int)Status.Deleted || status == (int)Status.Inactive)
            {
                if (branchService.CanDeactivateDelete(id) == -1)
                {
                    response.Success = false;
                    response.Message = "Branch is in use cannot be deactivated or deleted";
                    return Json(response);

                }
            }
            var res = branchService.ChangeStatus(id, status);
            if (res == 0)
            {
                response.Success = false;
                response.Message = "Error occurred while changing branch status";
            }
            else
            {
                response.Success = true;
                response.Message = "Branch status changed successfully";
            }
            return Json(response);
        }
    }
}
