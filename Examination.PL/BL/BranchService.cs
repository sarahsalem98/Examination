using AutoMapper;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;
        public BranchService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BranchService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public List<BranchMV> GetByStatus(int status)
        {
            try
            {
                var branches = _unitOfWork.BranchRepo.GetAll(b=>b.Status==status);
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
