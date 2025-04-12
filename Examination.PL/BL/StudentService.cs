﻿using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class StudentService:IStudentService
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
                var std = _mapper.Map<Student>(student);
                _unitOfWork.StudentRepo.Insert(std);
                result = _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public PaginatedData<StudentMV> GetAllPaginated(StudentSearchMV studentSearch , int PageSize=10 , int Page=1)
        {
            try
            {
                List<StudentMV> studentMVs = new List<StudentMV>();
                List<Student> data= _unitOfWork.StudentRepo.GetAll(
                           s=>( studentSearch.TrackType==null || s.TrackType==studentSearch.TrackType)&&
                           ( studentSearch.BranchId==null|| s.DepartmentBranch.BranchId==studentSearch.BranchId)&&
                           (studentSearch.Status==null ||s.User.Status==studentSearch.Status)&&
                           (studentSearch.DepartmentId==null||studentSearch.DepartmentId==s.DepartmentBranch.DepartmentId)&&
                           (String.IsNullOrEmpty (studentSearch.Name)||
                           ( !String.IsNullOrEmpty(s.User.FirstName)&&s.User.FirstName.ToLower().Trim().Contains(studentSearch.Name))||
                           (!String.IsNullOrEmpty(s.User.LastName) && s.User.LastName.ToLower().Trim().Contains(studentSearch.Name)))

                    , "User,DepartmentBranch.Department,DepartmentBranch.Branch").ToList();
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
    }
}
