using AutoMapper;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class GeneratedExamQService: IGeneratedExamQService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GeneratedExamQService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BranchService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public List< GeneratedExamQMV> GetGeneratedExam( int GeneratedExam_id)
        {
            try
            {
                var questions = _unitOfWork.GeneratedExamQRepo.GetAll(g=>g.GeneratedExamId==GeneratedExam_id,
                    "ExamQs,ExamStudentAnswers").ToList();
           
                var questionsMV=_mapper.Map<List< GeneratedExamQMV>>(questions);
                return  questionsMV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while adding new Branch ");
                return null;
            }
        }
    }
}
