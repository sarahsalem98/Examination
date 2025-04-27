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
                var instructorCourses = unitOfWork.InstructorCourseRepo.GetAll(ic => ic.InstructorId == CreatedBy && ic.Course.Exams.Any(e => e.Id == ExamId),
                    "Course.Exams").ToList();
                var departmentBranchId = unitOfWork.DepartmentBranchRepo.GetAll(d => d.DepartmentId == DepartmentId && d.BranchId == BranchId).FirstOrDefault()?.Id;
                if (departmentBranchId == null)
                {

                    throw new Exception("Department Branch not found");

                }
                var generatedExamExsist = unitOfWork.GeneratedExamRepo.GetAll(e => e.ExamId == ExamId && e.DepartmentBranchId == departmentBranchId);
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
                var res = unitOfWork.GeneratedExamRepo.GenerateExam(ExamId, DepartmentId, BranchId, NumsTS, NumsMCQ, CreatedBy, TakenDate, takenTime);
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
                ).OrderByDescending(e => e.CreatedAt).ToList();

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
        public int UpdateGeneratedExam(int GeneratedExamID, DateOnly TakenDate, TimeOnly takenTime)
        {
            try
            {
                var exsistingexam = unitOfWork.GeneratedExamRepo.FirstOrDefault(e => e.Id == GeneratedExamID);
                if (exsistingexam == null)
                {
                    return 0;
                }
                else
                {
                    exsistingexam.TakenDate = TakenDate;
                    exsistingexam.TakenTime = takenTime;
                    unitOfWork.GeneratedExamRepo.Update(exsistingexam);
                    var res = unitOfWork.Save();
                    if (res > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                logger.LogError("error occuired while Updating exams");
                return 0;
            }
        }
        public GeneratedExamMV GetByID(int GeneratedExamId)
        {
            try
            {
                var exam = unitOfWork.GeneratedExamRepo.FirstOrDefault(e => e.Id == GeneratedExamId, "DepartmentBranch");



                if (exam != null)
                {
                    var GeneratedExam = mapper.Map<GeneratedExamMV>(exam);
                    return GeneratedExam;
                }
                else
                {
                    return null;

                }
            }
            catch
            {
                logger.LogError("error occuired while getting exam");
                return null;
            }
        }

        public List<GeneratedExamMV> CommingExam(string userIdString)
        {
            try
            {

                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                {
                    logger.LogWarning("UserId is not found or invalid in the HttpContext.");
                    return null;
                }

                // Get the student branch department ID from the database
                var studentBranchDepId = unitOfWork.StudentRepo.FirstOrDefault(s => s.UserId == userId).DepartmentBranchId;

                if (studentBranchDepId == null)
                {
                    logger.LogWarning("Student's Department Branch ID is not found.");
                    return null;
                }

                //get Student comming exam
                var commingExam = unitOfWork.GeneratedExamRepo
                    .GetAll(e => e.DepartmentBranchId == studentBranchDepId,"Exam"
                    ).ToList()
                    .Where(e =>
                    {
                        var startDate = e.TakenDate.ToDateTime(e.TakenTime);
                        var endDate = startDate.AddMinutes(e.Exam.Duration);
                        return startDate > DateTime.Now || (startDate <= DateTime.Now && DateTime.Now <= endDate);
                    })
                    .OrderBy(e => e.TakenDate.ToDateTime(e.TakenTime))
                    .ToList();

                if (!commingExam.Any())
                {
                    logger.LogWarning("No upcoming exam found for the student.");
                    return null;
                }

                var commingExamList = mapper.Map<List<GeneratedExamMV>>(commingExam);
                return commingExamList;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error occurred while getting the comming exam");
                return null;
            }
        }

    }
}
