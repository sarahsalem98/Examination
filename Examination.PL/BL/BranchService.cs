using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BranchService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public int Add(BranchMV branch, List<int> DepIds)
        {
            int result = 0;
            try
            {
                var branchExist = _unitOfWork.BranchRepo.FirstOrDefault(u => u.Id == branch.Id);
                if (branchExist == null)
                {
                    //check department exist
                    var departments = _unitOfWork.DepartmentRepo.GetAll(d => DepIds.Contains(d.Id)).ToList();
                    if (departments.Count != DepIds.Count)
                    {
                        _logger.LogWarning("Some departments could not be found.");
                    }
                    //add
                    //1-add branch
                    var newbranch = _mapper.Map<Branch>(branch);
                    _unitOfWork.BranchRepo.Insert(newbranch);
                    _unitOfWork.Save();
                    //2-add deprtments
                    foreach (Department dep in departments)
                    {
                        var depbranch = new DepartmentBranch()
                        {
                            BranchId = newbranch.Id,
                            DepartmentId = dep.Id
                        };
                        _unitOfWork.DepartmentBranchRepo.Insert(depbranch);
                    }

                    result = _unitOfWork.Save();
                }
                else
                {
                    result = -1;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while adding new Branch ");
                return 0;
            }

        }

        public BranchMV GetById(int id)
        {
            try
            {
                BranchMV branchMV = new BranchMV();
                var branch = _unitOfWork.BranchRepo.FirstOrDefault(b => b.Id == id, "DepartmentBranch.Department");
                if (branch == null)
                {
                    return branchMV;
                }
                branchMV = _mapper.Map<BranchMV>(branch);
                return branchMV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving Branch data ");
                return null;
            }
        }


        public int Update(BranchMV branch)
        {
            int result = 0;
            try
            {
                var branchExist = _unitOfWork.BranchRepo.FirstOrDefault(b => b.Id == branch.Id, "DepartmentBranches");
                if (branchExist != null)
                {
                    branch.CreatedAt = branchExist.CreatedAt;
                    branch.CreatedBy = branchExist.CreatedBy;

                    var userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                    branch.UpdatedAt = DateTime.Now;
                    branch.UpdatedBy = userId != null ? int.Parse(userId) : (int?)null;

                    _mapper.Map(branch, branchExist);
                    _unitOfWork.BranchRepo.Update(branchExist);
                    result = _unitOfWork.Save();
                }
                else
                {
                    result = -1;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating branch data");
                return 0;
            }
        }

        public int ChangeStatus(int id, int status)
        {
            try
            {
                var branch = _unitOfWork.BranchRepo.FirstOrDefault(b => b.Id == id);
                if (branch != null)
                {
                    branch.Status = status;
                    branch.UpdatedAt = DateTime.Now;

                    var userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                    branch.UpdatedBy = userId != null ? int.Parse(userId) : (int?)null;

                    _unitOfWork.BranchRepo.Update(branch);
                    return _unitOfWork.Save();
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing branch status");
                return 0;
            }
        }

        public List<BranchMV> GetByStatus(int status)
        {
            try
            {
                var branches = _unitOfWork.BranchRepo.GetAll(b => b.Status == status);
                if (branches == null || branches.Count() == 0)
                {
                    _logger.LogWarning($"No branches found with status {status}");
                    return new List<BranchMV>();
                }
                var branchMVs = _mapper.Map<List<BranchMV>>(branches);
                return branchMVs;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in BranchService.GetByStatus", ex);
            }

        }


    }
}
