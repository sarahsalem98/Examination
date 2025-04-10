using AutoMapper;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public List<DepartmentMV> GetByStatus(int status)
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepo.GetAll(d=>d.Status==status);
                if (departments == null || !departments.Any())
                {
                    return new List<DepartmentMV>();
                }
                var departmentMVs = _mapper.Map<List<DepartmentMV>>(departments);
                return departmentMVs;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByStatus DepartmentService");
                 return null;
            }
        }
    }
}
