using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class InstructorService:IInstructorService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<InstructorService> logger;
        public InstructorService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<InstructorService> _logger)
        {

            unitOfWork = _unitOfWork;
            mapper = _mapper;
            logger = _logger;
        }

        public PaginatedData<InstructorMV> GetAllPaginated(InstructorSearchMV search, int page = 1, int pagesize = 10)
        {
           List<InstructorMV > instructorsMV = new List<InstructorMV>();
            List<Instructor> instructors = unitOfWork.InstructorRepo.GetAll(
                i=>(i.IsExternal==search.IsExternal||i==null)&&
                (i.na),
                "GeneratedExams,InstructorCourses"
                ).ToList();
           

        }
    }
}
