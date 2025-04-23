using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

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
                var instructorCourses = unitOfWork.InstructorCourseRepo.GetAll(ic=>ic.InstructorId==CreatedBy&&ic.Course.Exams.Any(e=>e.Id==ExamId),
                    "Course.Exams").ToList();
                var departmentBranchId = unitOfWork.DepartmentBranchRepo.GetAll(d => d.DepartmentId == DepartmentId && d.BranchId == BranchId).FirstOrDefault()?.Id;
                if (departmentBranchId == null)
                {

                    throw new Exception("Department Branch not found");

                }
                var generatedExamExsist = unitOfWork.GeneratedExamRepo.GetAll(e => e.ExamId == ExamId &&e.DepartmentBranchId==departmentBranchId);
                if (instructorCourses.Any(ic => DateOnly.FromDateTime(ic.EndDate.Value) > TakenDate))
                {
                   
                    return -1;
                }
                else if (generatedExamExsist != null)
                {
                  
                    return -2;
                }

                var DepartmentBranchCourse = unitOfWork.InstructorCourseRepo.FirstOrDefault(ic => ic.Course.Exams.Any(e => e.Id == ExamId) && ic.InstructorId == CreatedBy,
                   "Course.Exams");
                if (DepartmentBranchCourse == null)
                {
                    return 0;
                }
                DepartmentBranchCourse.LastGeneratedExamType = instructorCourses
                                                    .SelectMany(ic => ic.Course.Exams)
                                                   .Where(e => e.Id == ExamId)
                                                   .Select(e => e.Type.Trim().ToLower())
                                                    .FirstOrDefault();
                unitOfWork.InstructorCourseRepo.Update(DepartmentBranchCourse);

                unitOfWork.Save();
                var res = unitOfWork.GeneratedExamRepo.GenerateExam(ExamId,DepartmentId,BranchId,NumsTS,NumsMCQ,CreatedBy,TakenDate,takenTime);
                return 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error occurred while generating exams");
                return 0;

            }

        }
        public PaginatedData<GeneratedExamMV> GetAllPaginated(int instructor_id, GeneratedExamSearchMV search, int PageSize = 10, int Page = 1)
        {

            try
            {
                List<GeneratedExamMV> exams = new List<GeneratedExamMV>();
                List<GeneratedExam> data = unitOfWork.GeneratedExamRepo.GetAll(
                    e =>
                        e.DepartmentBranch.InstructorCourses.Any(ic => ic.Instructor.UserId == instructor_id) &&
                        (search.CourseId == null || e.Exam.CourseId == search.CourseId) &&
                        (search.DepartmentId == null || e.DepartmentBranch.DepartmentId == search.DepartmentId) &&
                        (search.BranchId == null || e.DepartmentBranch.BranchId == search.BranchId) &&
                        (search.ExamType == null || e.Exam.Type == search.ExamType) &&
                        (
                            String.IsNullOrEmpty(search.Name) ||
                            (!String.IsNullOrEmpty(e.Exam.Name) &&
                             e.Exam.Name.ToLower().Trim().Contains(search.Name.ToLower().Trim()))
                        ),
                    "DepartmentBranch.InstructorCourses.Instructor,DepartmentBranch.Department,DepartmentBranch.Branch,Exam.Course"
                ).ToList();

                exams = mapper.Map<List<GeneratedExamMV>>(data);
                int TotalCounts = exams.Count();
                if (TotalCounts > 0)
                {
                    exams = exams.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
                PaginatedData<GeneratedExamMV> paginatedData = new PaginatedData<GeneratedExamMV>
                {
                    Items = exams,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error occuired while getting all exams");
                return null;
            }
        }
    }
}
