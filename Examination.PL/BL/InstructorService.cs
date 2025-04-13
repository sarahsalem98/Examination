using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using System.Drawing.Printing;

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
            try
            {
                List<InstructorMV> instructorsMVs = new List<InstructorMV>();

                var instructors = unitOfWork.InstructorRepo.GetAll(
                    i =>(search.IsExternal == null || i.IsExternal == search.IsExternal) &&
                   (search.status==null||i.User.Status==search.status)&&
                     i.User != null &&
                     (string.IsNullOrEmpty(search.Name) ||
                   (!string.IsNullOrEmpty(i.User.FirstName) && i.User.FirstName.ToLower().Trim().Contains(search.Name.ToLower().Trim())) ||
                  (!string.IsNullOrEmpty(i.User.LastName) && i.User.LastName.ToLower().Trim().Contains(search.Name.ToLower().Trim()))
                   ),
                      "GeneratedExams,InstructorCourses,User"
                     ).OrderByDescending(i => i.User.CreatedAt).ToList();


                instructorsMVs = mapper.Map<List<InstructorMV>>(instructors);
                int TotalCounts = instructorsMVs.Count();

                if (TotalCounts > 0)
                {
                    instructorsMVs = instructorsMVs.Skip((page - 1) * pagesize).Take(pagesize).ToList();
                }

                PaginatedData<InstructorMV> paginatedData = new PaginatedData<InstructorMV>
                {
                    TotalCount = TotalCounts,
                    Items = instructorsMVs,
                    PageSize = pagesize,
                    CurrentPage = page
                };

                return paginatedData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving instructor data.");
                return null;
            }
        }

    }
}
