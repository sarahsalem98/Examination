using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using NuGet.Packaging;

namespace Examination.PL.BL
{
    public class InstructorCourseService : IInstructorCourseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<InstructorService> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public InstructorCourseService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<InstructorService> _logger, IHttpContextAccessor _httpContextAccessor)
        {

            unitOfWork = _unitOfWork;
            mapper = _mapper;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }

        public int CompleteCourse(int DepartmentBranchId, int instructor_id, int course_id)
        {
            try
            {
                var DepartmentBranchCourse = unitOfWork.InstructorCourseRepo.FirstOrDefault(ic => ic.DepartmentBranchId == DepartmentBranchId && ic.CourseId == course_id && ic.Instructor.UserId == instructor_id,
                    "DepartmentBranch.Students.StudentCourses,Course,Course.Exams,Course.Exams.GeneratedExams,Instructor,DepartmentBranch,DepartmentBranch.Department,DepartmentBranch.Branch,DepartmentBranch.Students,DepartmentBranch.Students.ExamStudentGrades,DepartmentBranch.Students.ExamStudentGrades.GeneratedExam.Exam");
                if (DepartmentBranchCourse == null)
                {
                    return 0;
                }
                var TakenDateExamForCourse = DepartmentBranchCourse.Course.Exams.Where(e=>e.CourseId==course_id)
                    .SelectMany(e=>e.GeneratedExams.Select(ge=>ge.TakenDate)).FirstOrDefault();


                if (DepartmentBranchCourse.EndDate > DateTime.Now)
                {
                    //means course doasn't end (tested)
                    return -2;
                }
                else if (TakenDateExamForCourse == null || DepartmentBranchCourse.LastGeneratedExamType == null)
                {
                    //means instructor doasn't generate exam (tested)
                    return -3;
                }
                else if (TakenDateExamForCourse >= DateOnly.FromDateTime(DateTime.Now) || (DepartmentBranchCourse.FinalPassedStudentCount == null && DepartmentBranchCourse.CorrectivePassedStudentCount == null))
                {
                    //means student doeasn't have the exam(tested)
                    return -4;
                }
                else if ((DepartmentBranchCourse.TotalStudents != DepartmentBranchCourse.FinalPassedStudentCount)
                  && DepartmentBranchCourse.LastGeneratedExamType.Trim().ToLower() == "final")
                {
                    //means failed students doeasn't have corrective exam (tested)
                    return -5;
                }
                else if (DepartmentBranchCourse.IsCompleted == 1 )

                {
                    //means he already put the grades
                    return -6;
                }
                else
                {
                    foreach (var student in DepartmentBranchCourse.DepartmentBranch.Students)

                    {
                        var studentCourse = student.StudentCourses.FirstOrDefault(sc => sc.CourseId == course_id);
                        if (studentCourse != null)
                        {
                            var finalgradepercent = student.ExamStudentGrades.FirstOrDefault(es => es.GeneratedExam?.Exam?.CourseId == course_id)?.GradePercent ?? 0;
                            studentCourse.FinalGradePercent = (int)finalgradepercent;
                            unitOfWork.StudentCourseRepo.Update(studentCourse);
                        }
                    }
                    DepartmentBranchCourse.IsCompleted = 1;
                    unitOfWork.InstructorCourseRepo.Update(DepartmentBranchCourse);
                 
                    var res = unitOfWork.Save();
                    if(res>0)
                    {
                        return 1;
                    }else
                    {
                        return -1;
                    }
                }
              
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Complete Courses");
                return 0;

            }


        }

        public PaginatedData<InstructorCourseMV> GetCourseByInstructorPaginated(int Instructor_Id, CourseSearchMV courseSearch, int Page = 1, int PageSize = 10)
        {
            try
            {
                List<InstructorCourseMV> instructorcourseMV = new List<InstructorCourseMV>();


                List<InstructorCourse> data = unitOfWork.InstructorCourseRepo.GetAll(ic=>ic.Instructor.UserId==Instructor_Id
                &&(courseSearch.CourseId==null||ic.CourseId==courseSearch.CourseId)
                &&(courseSearch.DepartmentId==null||ic.DepartmentBranch.DepartmentId==courseSearch.DepartmentId)
               && (courseSearch.BranchId == null || ic.DepartmentBranch.BranchId == courseSearch.BranchId)
               &&(courseSearch.Status==null||(ic.IsCompleted??0)==(int)courseSearch.Status)
               && (string.IsNullOrEmpty(courseSearch.Name) ||
                   ic.Course.Name.ToLower().Trim().Contains(courseSearch.Name.ToLower().Trim()))


         , "Course,Instructor,DepartmentBranch,DepartmentBranch.Department,DepartmentBranch.Branch").Where(ic=>ic.DepartmentBranch.Department.Status==(int)Status.Active&&ic.Course.Status== (int)Status.Active
        &&ic.DepartmentBranch.Branch.Status== (int)Status.Active).ToList();
                instructorcourseMV = mapper.Map<List<InstructorCourseMV>>(data);
                int TotalCounts = instructorcourseMV.Count();
                if (TotalCounts > 0)
                {
                    instructorcourseMV = instructorcourseMV.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
               ;

                PaginatedData<InstructorCourseMV> paginatedData = new PaginatedData<InstructorCourseMV>
                {
                    Items = instructorcourseMV,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in fetch Courses ");
                return null;

            }

        }
    }
}
