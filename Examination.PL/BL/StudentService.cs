using AutoMapper;
using BCrypt.Net;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentService> logger)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public int Add(StudentMV student)
        {
            int result = 0;
            try
            {
                var userExist = _unitOfWork.UserRepo.FirstOrDefault(u => u.Email == student.User.Email);
                if (userExist == null)
                { 
                    var departmentBranchId = _unitOfWork.DepartmentBranchRepo.GetAll(d => d.DepartmentId == student.DepartmentId && d.BranchId == student.BranchId).FirstOrDefault()?.Id;
                    if (departmentBranchId == null)
                    {

                        throw new Exception("Department Branch not found");

                    }
                    student.DepartmentBranchId = (int)departmentBranchId;
                    var newStudent = _mapper.Map<Student>(student);
                    newStudent.User.CreatedAt = DateTime.Now;
                    newStudent.User.Password = PasswordHelper.HashPassword("123456");
                    newStudent.User.CreatedBy = 1;
                    newStudent.User.Status = (int)Status.Active;
                    newStudent.User.UserTypes.Add(_unitOfWork.UserTypeRepo.FirstOrDefault(s => s.TypeName == Constants.UserTypes.Student));
                    _unitOfWork.StudentRepo.Insert(newStudent);
                    result = _unitOfWork.Save();
                    
                }
                else
                {
                    result = -1;
                }
                return result;
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, "error occuired while adding new student in admin area");
                return 0;
            }
        }
        public PaginatedData<StudentMV> GetAllPaginated(StudentSearchMV studentSearch, int PageSize = 10, int Page = 1)
        {
            try
            {
                List<StudentMV> studentMVs = new List<StudentMV>();
                List<Student> data = _unitOfWork.StudentRepo.GetAll(
                           s => (studentSearch.TrackType == null || s.TrackType == studentSearch.TrackType) &&
                           (studentSearch.BranchId == null || s.DepartmentBranch.BranchId == studentSearch.BranchId) &&
                           (studentSearch.Status == null || s.User.Status == studentSearch.Status) &&
                           (studentSearch.DepartmentId == null || studentSearch.DepartmentId == s.DepartmentBranch.DepartmentId) &&
                           (String.IsNullOrEmpty(studentSearch.Name) ||
                           (!String.IsNullOrEmpty(s.User.FirstName) && s.User.FirstName.ToLower().Trim().Contains(studentSearch.Name)) ||
                           (!String.IsNullOrEmpty(s.User.LastName) && s.User.LastName.ToLower().Trim().Contains(studentSearch.Name)))

                    , "User,DepartmentBranch.Department,DepartmentBranch.Branch").OrderByDescending(s=>s.User.CreatedAt).ToList();
                studentMVs = _mapper.Map<List<StudentMV>>(data);
                int TotalCounts = studentMVs.Count();
                if (TotalCounts > 0)
                {
                    studentMVs = studentMVs.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

                }
                PaginatedData<StudentMV> paginatedData = new PaginatedData<StudentMV>
                {
                    Items = studentMVs,
                    TotalCount = TotalCounts,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
                return paginatedData;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving student data in admin area");
                return null;

            }
        }

        public StudentMV GetById(int id)
        {
            try
            {
                StudentMV studentMV = new StudentMV();
                var student = _unitOfWork.StudentRepo.FirstOrDefault(s => s.Id == id, "User,DepartmentBranch.Department,DepartmentBranch.Branch");
                if (student == null)
                {
                    return studentMV;
                }
                studentMV = _mapper.Map<StudentMV>(student);
                return studentMV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occuired while retriving student data in admin area");
                return null;
            }
        }
    }
}
