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

        public int Add(BranchMV branch)
        {
            int result = 0;
            try
            {
                var branchExist = _unitOfWork.BranchRepo.FirstOrDefault(u => u.Id == branch.Id);
                if (branchExist == null)
                {
                    //1-add branch
                    var newbranch = _mapper.Map<Branch>(branch);
                    newbranch.CreatedAt = DateTime.Now;
                    newbranch.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    newbranch.Status = (int)Status.Active;
                    _unitOfWork.BranchRepo.Insert(newbranch);
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
                var branch = _unitOfWork.BranchRepo.FirstOrDefault(b => b.Id == id);
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
                var branchExist = _unitOfWork.BranchRepo.FirstOrDefault(b => b.Id == branch.Id);
                if (branchExist != null)
                {
                   

                    var userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                    branchExist.UpdatedAt = DateTime.Now;
                    branchExist.UpdatedBy = userId != null ? int.Parse(userId) : (int?)null;
                    branchExist.Name = branch.Name;
                    branchExist.Location = branch.Location;
                  


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

        public PaginatedData<BranchMV> GetAllPaginated(BranchSearchMV branchSearch, int PageSize = 10, int Page = 1)
        {
            try
            {
                var query = _unitOfWork.BranchRepo.GetAll(
                    b =>
                        (string.IsNullOrWhiteSpace(branchSearch.Name) || b.Name.ToLower().Contains(branchSearch.Name.ToLower().Trim())) &&
                        (string.IsNullOrWhiteSpace(branchSearch.Location) || b.Location.ToLower().Contains(branchSearch.Location.ToLower().Trim())) &&
                        (branchSearch.Status == null || b.Status == branchSearch.Status)
                ).OrderByDescending(b => b.CreatedAt);

                int totalCount = query.Count();

                var paginatedBranches = query
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                var branchMVs = _mapper.Map<List<BranchMV>>(paginatedBranches);

                return new PaginatedData<BranchMV>
                {
                    Items = branchMVs,
                    TotalCount = totalCount,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving branch data.");
                return null;
            }
        }

        public List<String> GetDistinctBranchLocations()
        {
            try
            {
                var locations = _unitOfWork.BranchRepo.GetAll()
                    .Where(b => b.Status == (int)Status.Active)
                    .Select(b => b.Location)
                    .Distinct()
                    .ToList();
                return locations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving distinct branch locations.");
                return null;
            }
        }
        public int CanDeactivateDelete(int id)
        {

            try
            {
                var unFinishedCourse = _unitOfWork.StudentCourseRepo.FirstOrDefault(s => s.Student.DepartmentBranch.BranchId == id && s.FinalGradePercent == null, "Student.DepartmentBranch");
                return unFinishedCourse != null ? -1 : 1;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the branch can be deactivated or deleted.");
                return 0;
            }
        }
        public List<BranchMV> GetBranchesByInstructor(int instructor_id)
        {
            try 
            {
              List<  BranchMV> branches = new List< BranchMV>();
                var data = _unitOfWork.BranchRepo.GetAll(
                    b => b.DepartmentBranches.Any(db => db.Status == (int)Status.Active && db.InstructorCourses.Any(ic => ic.Instructor.UserId == instructor_id))
                    , "DepartmentBranches,DepartmentBranches.InstructorCourses.Instructor"

                   ).Distinct().ToList();
                branches=_mapper.Map<List<BranchMV>>(data);
                return branches;
            }
           catch (Exception ex)
            {
                _logger.LogError( "An error occurred while loading branches.");
                return null;
            }
        }

    }
}
