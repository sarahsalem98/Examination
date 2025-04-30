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
                var departmentBranchId = unitOfWork.DepartmentBranchRepo.FirstOrDefault(d => d.DepartmentId == DepartmentId && d.BranchId == BranchId)?.Id;
                var instructorCourses = unitOfWork.InstructorCourseRepo.FirstOrDefault(ic => ic.InstructorId == CreatedBy &&ic.DepartmentBranchId==departmentBranchId&& ic.Course.Exams.Any(e => e.Id == ExamId),
                    "Course.Exams");
               
                if (departmentBranchId == null)
                {

                    throw new Exception("Department Branch not found");

                }
             

                var generatedExamExsist = unitOfWork.GeneratedExamRepo.FirstOrDefault(e => e.ExamId == ExamId && e.DepartmentBranchId == departmentBranchId&&e.TakenDate.Year == TakenDate.Year);
                if(instructorCourses.IsCompleted==1)
                {
                    return -3;
                }
               else if (generatedExamExsist != null)
                {

                    return -2;
                }

                else if (DateOnly.FromDateTime(instructorCourses.EndDate.Value) > TakenDate)
                {

                    return -1;
                }
                 

              
                instructorCourses.LastGeneratedExamType = instructorCourses.Course.Exams.FirstOrDefault(e => e.Id == ExamId)?.Type.ToLower();
                                                 
                unitOfWork.InstructorCourseRepo.Update(instructorCourses);

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
                var exams = unitOfWork.GeneratedExamRepo.GetAll(e => e.DepartmentBranchId == exsistingexam.DepartmentBranchId && e.ExamId == exsistingexam.ExamId);
                if(exams.Any(e=>e.TakenDate.Year==TakenDate.Year&&e.Id!=exsistingexam.Id))
                 {
                    return -1;
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
                var exam = unitOfWork.GeneratedExamRepo.FirstOrDefault(e => e.Id == GeneratedExamId , "DepartmentBranch,GeneratedExamQs.ExamQs,Exam");



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
                }

                // Get the student branch department ID from the database
                var student= unitOfWork.StudentRepo.FirstOrDefault(s => s.UserId == int.Parse(userIdString), "StudentCourses");
                var studentBranchDepId = student.DepartmentBranchId;

                if (studentBranchDepId == null)
                {
                    logger.LogWarning("Student's Department Branch ID is not found.");
                    
                }

                //get Student comming exam
                var commingExam = unitOfWork.GeneratedExamRepo
                    .GetAll(e => e.DepartmentBranchId == studentBranchDepId  , "Exam,ExamStudentGrades"
                    ).ToList()
                    .Where(e =>
                    {
                        var startDate = e.TakenDate.ToDateTime(e.TakenTime);
                        var endDate = startDate.AddMinutes(e.Exam.Duration);
                        var studentExamTaken = e.ExamStudentGrades.FirstOrDefault(d=>d.GeneratedExamId==e.Id&&d.StudentId==student.Id);
                        return (startDate > DateTime.Now || (startDate <= DateTime.Now && DateTime.Now <= endDate)) &&(studentExamTaken == null);
                    })
                    .OrderBy(e => e.TakenDate.ToDateTime(e.TakenTime))
                    .ToList();

                if (!commingExam.Any())
                {
                    logger.LogWarning("No upcoming exam found for the student.");
                    
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

        public PaginatedData<GeneratedExamMV> GetPreviousExams(string userIdString, GeneratedExamSearchMV search, int pageSize = 10, int page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                {
                    logger.LogWarning("UserId is not found or invalid in the HttpContext.");
                    return new PaginatedData<GeneratedExamMV> { Items = new List<GeneratedExamMV>(), TotalCount = 0 };
                }

                var student = unitOfWork.StudentRepo.FirstOrDefault(s => s.UserId == userId);

                if (student == null)
                {
                    logger.LogWarning("Student not found.");
                    return new PaginatedData<GeneratedExamMV> { Items = new List<GeneratedExamMV>(), TotalCount = 0 };
                }

                var studentBranchDepId = student.DepartmentBranchId;

                var examsQuery = unitOfWork.GeneratedExamRepo.GetAll(
                    e =>
                        e.DepartmentBranchId == studentBranchDepId &&
                        (search.ExamType == null || e.Exam.Type == search.ExamType) &&
                        (string.IsNullOrEmpty(search.Name) ||
                         (!string.IsNullOrEmpty(e.Exam.Name) && e.Exam.Name.ToLower().Trim().Contains(search.Name.ToLower().Trim()))
                        ),
                    includeProperties: "Exam,ExamStudentGrades"
                ).ToList()
                .Where(e =>
                {
                    var startDate = e.TakenDate.ToDateTime(e.TakenTime);
                    var endDate = startDate.AddMinutes(e.Exam.Duration);
                    return endDate < DateTime.Now;
                })
                .OrderByDescending(e => e.TakenDate.ToDateTime(e.TakenTime))
                .ToList();

                // Map exams
                var mappedExams = examsQuery.Select(e => new
                {
                    ExamMV = mapper.Map<GeneratedExamMV>(e),
                    ExamId = e.Id,
                    Grade = e.ExamStudentGrades
                 .Where(g => g.StudentId == student.Id && g.GeneratedExamId == e.Id)
                 .FirstOrDefault()?.GradePercent
                });

                // Filtered & paginated
                int totalCount = mappedExams.Count();
                var paginatedExams = mappedExams.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Store Grades in HttpContext.Items
                httpContextAccessor.HttpContext.Items["ExamGrades"] = paginatedExams.ToDictionary(e => e.ExamId, e => e.Grade);

                return new PaginatedData<GeneratedExamMV>
                {
                    Items = paginatedExams.Select(e => e.ExamMV).ToList(),
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving previous exams.");
                return new PaginatedData<GeneratedExamMV> { Items = new List<GeneratedExamMV>(), TotalCount = 0 };
            }
        }


    }



}

