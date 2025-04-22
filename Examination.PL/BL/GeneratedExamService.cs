using AutoMapper;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;

namespace Examination.PL.BL
{
    public class GeneratedExamService : IGeneratedExamService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<InstructorService> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public GeneratedExamService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<InstructorService> _logger, IHttpContextAccessor _httpContextAccessor)
        {

            unitOfWork = _unitOfWork;
            mapper = _mapper;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        public int GenerateExam(int ExamId, int DepartmentId, int BranchId, int NumsTS, int NumsMCQ, int CreatedBy, DateOnly TakenDate, TimeOnly takenTime)
        {
            try
            {
                var res = unitOfWork.GeneratedExamRepo.GenerateExam(ExamId,DepartmentId,BranchId,NumsTS,NumsMCQ,CreatedBy,TakenDate,takenTime);
                if(res>0)
                {
                    return 1;
                }else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Generating Exam");
                return 0;

            }
        }
    }
}
