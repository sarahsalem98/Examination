using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
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
        private readonly IHttpContextAccessor httpContextAccessor;
        public InstructorService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<InstructorService> _logger , IHttpContextAccessor _httpContextAccessor)
        {

            unitOfWork = _unitOfWork;
            mapper = _mapper;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        public int ChangeStatus(int Id, int status)
        {
            try
            {

                var res = 0;
                var instructor = unitOfWork.InstructorRepo.FirstOrDefault(i => i.Id == Id, "GeneratedExams,InstructorCourses,User");
                if(instructor!=null)
                {
                    instructor.User.Status = status;
                    instructor.User.UpdatedAt = DateTime.Now;
                    instructor.User.UpdatedBy = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value); ;
                    unitOfWork.InstructorRepo.Update(instructor);
                    res = unitOfWork.Save();
                    return res;
                }
                return -1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while Adding instructor data.");
                return 0;
            }
        }
        public InstructorMV getById(int Id)
        {
            try
            {
                InstructorMV instructorMV = new InstructorMV();
              var instructor=unitOfWork.InstructorRepo.FirstOrDefault(i => i.Id ==Id, "GeneratedExams,InstructorCourses,User");
                if(instructor == null)
                {
                    return instructorMV;
                }
                else
                {
                    instructorMV=mapper.Map<InstructorMV>(instructor);
                    return instructorMV;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while Adding instructor data.");
                return null;
            }
        }
        public int Update(InstructorMV instructor)
        {
            try
            {
                var res = 0;
                var oldinstructor=unitOfWork.InstructorRepo.FirstOrDefault(i=>i.Id==instructor.Id, "GeneratedExams,InstructorCourses,User");
                if (oldinstructor == null)
                {
                    return -1;
                }
                else
                {

                    oldinstructor.User.UpdatedAt = DateTime.Now;
                    oldinstructor.User.UpdatedBy = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    oldinstructor.User.Email = instructor.User.Email;
                    oldinstructor.User.FirstName = instructor.User.FirstName;
                    oldinstructor.User.LastName = instructor.User.LastName;
                    oldinstructor.User.Phone = instructor.User.Phone;
                    oldinstructor.User.Age = instructor.User.Age;
                    oldinstructor.IsExternal = instructor.IsExternal;
            
                    unitOfWork.InstructorRepo.Update(oldinstructor);
                    res = unitOfWork.Save();
                    return res;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while Updating instructor data.");
                return 0;
            }
        }
        public int Add(InstructorMV instructor)
        {
            try
            {
                var res = 0;
                var user = unitOfWork.UserRepo.FirstOrDefault(u => u.Email == instructor.User.Email);
                if (user != null)
                {
                    return -1;
                }else
                {
                    var NewInstructor=mapper.Map<Instructor>(instructor); 
                    NewInstructor.User.CreatedAt = DateTime.Now;
                    NewInstructor.User.CreatedBy = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value); ;
                    NewInstructor.User.Email = instructor.User.Email;
                    NewInstructor.User.FirstName = instructor.User.FirstName;
                    NewInstructor.User.LastName = instructor.User.LastName;
                    NewInstructor.User.Phone = instructor.User.Phone;
                    NewInstructor.User.Age = instructor.User.Age;
                    NewInstructor.User.Password=PasswordHelper.HashPassword("123456");
                    NewInstructor.IsExternal=instructor.IsExternal;
                    NewInstructor.User.Status = (int)Status.Active;
                    NewInstructor.User.UserTypes.Add(unitOfWork.UserTypeRepo.FirstOrDefault(i=>i.TypeName==Constants.UserTypes.Instructor));
                    unitOfWork.InstructorRepo.Insert(NewInstructor);
                    res = unitOfWork.Save();
                    return res;
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while Adding instructor data.");
                return 0;
            }
        }

        public PaginatedData<InstructorMV> GetAllPaginated(InstructorSearchMV InstructorSearch, int page = 1, int pagesize = 10)
        {
            try
            {
                List<InstructorMV> instructorsMVs = new List<InstructorMV>();

                var instructors = unitOfWork.InstructorRepo.GetAll(
                    i =>(InstructorSearch.IsExternal == null || i.IsExternal == (bool)InstructorSearch.IsExternal) &&
                   (InstructorSearch.Status==null||i.User.Status==(int)InstructorSearch.Status)&&
                     (string.IsNullOrEmpty(InstructorSearch.Name) ||
                   (!string.IsNullOrEmpty(i.User.FirstName) && i.User.FirstName.ToLower().Trim().Contains(InstructorSearch.Name.ToLower().Trim())) ||
                  (!string.IsNullOrEmpty(i.User.LastName) && i.User.LastName.ToLower().Trim().Contains(InstructorSearch.Name.ToLower().Trim()))
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
