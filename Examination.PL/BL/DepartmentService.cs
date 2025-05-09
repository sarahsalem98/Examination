﻿using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Http;
using System.Drawing.Printing;

namespace Examination.PL.BL
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger , IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

        }
        public List<DepartmentMV> GetByStatus(int status)
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepo.GetAll(d=>d.Status==status);
                if (departments == null || !departments.Any())
                {
                    return new List<DepartmentMV>();
                }
                var departmentMVs = _mapper.Map<List<DepartmentMV>>(departments);
                return departmentMVs;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByStatus DepartmentService");
                 return null;
            }
        }


        public List<DepartmentMV> GetByBranch(int branchId)
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepo.GetAll(d => d.DepartmentBranches.Where(d=>d.Status==(int)Status.Active).Select(b => b.BranchId).Contains(branchId), "DepartmentBranches");
                departments = departments.Where(d => d.Status == (int)Status.Active).ToList();
                if (departments == null || !departments.Any())
                {
                    return new List<DepartmentMV>();
                }
                var departmentMVs = _mapper.Map<List<DepartmentMV>>(departments);
                return departmentMVs;


            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error in GetByBranch DepartmentService");
                return null;

            }       
        }
        public List<DepartmentMV> GetByBranchAndInstructor(int branchId,int instructorId)
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepo.GetAll(d => d.DepartmentBranches.Any(db=>db.BranchId==branchId&&db.InstructorCourses.Any(ic=>ic.Instructor.UserId==instructorId))
                , "DepartmentBranches,DepartmentBranches.InstructorCourses.Instructor");
                if (departments == null || !departments.Any())
                {
                    return new List<DepartmentMV>();
                }
                var departmentMVs = _mapper.Map<List<DepartmentMV>>(departments);
                return departmentMVs;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByBranchAndInstrictor DepartmentService");
                return null;

            }
        }


        public DepartmentMV GetById(int id)
        {
            try
            {
                DepartmentMV departmentMV= new DepartmentMV();
                var department = _unitOfWork.DepartmentRepo.FirstOrDefault(d => d.Id == id);
                return _mapper.Map<DepartmentMV>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetById DepartmentService");
                return null;
            }
        }



        public PaginatedData<DepartmentMV> GetAllPaginated(DepartmentSearchMV departmentSearch, int PageSize = 10, int Page = 1)
        {
            try
            {
                var data = _unitOfWork.DepartmentRepo.GetAll(
                    d =>
                        (string.IsNullOrEmpty(departmentSearch.Name) || d.Name.ToLower().Trim().Contains(departmentSearch.Name.ToLower().Trim())) &&
                        (departmentSearch.Status != (int)Status.Deleted ? d.Status != (int)Status.Deleted : d.Status == (int)Status.Deleted) &&
                        (departmentSearch.BranchId == null || d.DepartmentBranches.Any(b => b.BranchId == departmentSearch.BranchId)),
                    "DepartmentBranches.Branch"
                )
                .OrderByDescending(d => d.CreatedAt)
                .ToList();

                var totalCount = data.Count;

                var pagedDepartments = data
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                var result = pagedDepartments.Select(d => new DepartmentMV
                {
                    Id = d.Id,
                    Name = d.Name,
                    Capacity = d.Capacity,
                    Status = d.Status,
                    Description = d.Description,
                    selectedBranches = d.DepartmentBranches.Select(b => new SelectedBranches
                    {
                        Id = b.BranchId,
                        Name = b.Branch.Name,
                        CanBeRemoved = null,
                        Status=b.Status
                    }).ToList()
                }).ToList();

                return new PaginatedData<DepartmentMV>
                {
                    Items = result,
                    TotalCount = totalCount,
                    PageSize = PageSize,
                    CurrentPage = Page
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPaginated DepartmentService");
                return new PaginatedData<DepartmentMV>();
            }
        }


    





        public int Add(DepartmentMV department)
        {
            int result = 0;
            try
            {
             
                var existing = _unitOfWork.DepartmentRepo
                    .FirstOrDefault(d => d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

                if (existing != null)
                {
                    result = -1;
                }
                else
                {
                    
                    if (department.selectedBranches.Count()==0)
                    {
                        throw new Exception("At least one branch must be selected.");
                    }

                   
                    var newDepartment = _mapper.Map<Department>(department);
                    newDepartment.CreatedAt = DateTime.Now;
                    newDepartment.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    newDepartment.Status = (int)Status.Active;

                   
                    newDepartment.DepartmentBranches = department.selectedBranches.Select(branch => new DepartmentBranch
                    {
                        BranchId = branch.Id??0
                    }).ToList();

                    _unitOfWork.DepartmentRepo.Insert(newDepartment);
                    result = _unitOfWork.Save();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new department");
                return 0;
            }
        }



        public int Update(DepartmentMV department)
        {
            int result = 0;
            try
            {
                var departmentExist = _unitOfWork.DepartmentRepo
                    .FirstOrDefault(d => d.Id == department.Id, "DepartmentBranches");

                if (departmentExist != null)
                {
                    var nameExists = _unitOfWork.DepartmentRepo
                        .FirstOrDefault(d => d.Id != department.Id && d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

                    if (nameExists != null)
                    {
                        result = -1; 
                    }
                    else
                    {

                        if (department.selectedBranches.Count == 0)
                        {
                            throw new Exception("At least one branch must be selected.");
                        }

                        
                        department.CreatedAt = departmentExist.CreatedAt;
                        department.CreatedBy = departmentExist.CreatedBy;
                        department.Status = departmentExist.Status;
                        _mapper.Map(department, departmentExist);
                        departmentExist.UpdatedAt = DateTime.Now;
                        departmentExist.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                        departmentExist.Name = department.Name;
                        departmentExist.Description = department.Description;
                        departmentExist.Capacity = department.Capacity;
                    
                     
                        var existingBranchIds = departmentExist.DepartmentBranches.Select(db => db.BranchId).ToList();

                        
                        var branchesToRemove = departmentExist.DepartmentBranches
                            .Where(db => !department.selectedBranches.Select(d=>d.Id).Contains(db.BranchId))
                            .ToList();
                        foreach (var branch in branchesToRemove)
                        {
                            branch.Status = (int)Status.Deleted;
                        }

                        foreach (var branch in department.selectedBranches)
                        {
                            var existingBranch = departmentExist.DepartmentBranches
                                .FirstOrDefault(db => db.BranchId == branch.Id && db.Status == (int)Status.Deleted);

                            if (existingBranch != null)
                            {
                                existingBranch.Status = 1; 
                            }
                            else
                            {
                                if (!existingBranchIds.Contains(branch.Id ?? 0))
                                {
                                    departmentExist.DepartmentBranches.Add(new DepartmentBranch
                                    {
                                        BranchId = branch.Id ?? 0,
                                        DepartmentId = department.Id,
                                        Status = 1 
                                    });
                                }
                            }
                        }

                        _unitOfWork.DepartmentRepo.Update(departmentExist);
                        result = _unitOfWork.Save();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating department");
                return 0;
            }
        }








        public int ChangeStatus(int id, int status)
        {
            try
            {
                var department = _unitOfWork.DepartmentRepo.FirstOrDefault(d => d.Id == id);
                if (department != null)
                {
                    department.Status = status;
                    department.UpdatedAt = DateTime.Now;
                    department.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    _unitOfWork.DepartmentRepo.Update(department);
                    return _unitOfWork.Save();
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing department status");
                return 0;
            }
        }


        public DepartmentMV GetByIdWithBranches(int id)
        {
            try
            {
                var department = _unitOfWork.DepartmentRepo
                    .FirstOrDefault(d => d.Id == id, "DepartmentBranches.Branch");

                if (department == null)
                    return null;

                var model = _mapper.Map<DepartmentMV>(department);
                model.selectedBranches = department.DepartmentBranches.Select(b=>
                   {
                      var studentWithCoursesUnFinished = _unitOfWork.StudentCourseRepo.FirstOrDefault(s => s.Student.DepartmentBranch.DepartmentId == id && s.Student.DepartmentBranch.BranchId==b.BranchId&& s.FinalGradePercent == null, "Student.DepartmentBranch"); 
                   return new SelectedBranches

                   {
                       Id = b.BranchId,
                       Name = b.Branch.Name,
                       CanBeRemoved = studentWithCoursesUnFinished == null?true:false,
                       Status=b.Status
                    }; 
                   
                   }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdWithBranches - DepartmentService");
                return null;
            }
        }

        public int CanDeativateOrDelete(int id)
        {
            try
            {

               var unFinishedCourse=_unitOfWork.StudentCourseRepo.FirstOrDefault(s=>s.Student.DepartmentBranch.DepartmentId==id&& s.FinalGradePercent==null, "Student.DepartmentBranch");
               return unFinishedCourse != null ? -1 : 1;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CanDeativateAndDelete - DepartmentService");
                return 0;
            }

        }

        public int CanRemoveDepartmentFromBranch(int departmentId, int branchId)
        {
            try
            {
                var unFinishedCourse = _unitOfWork.StudentCourseRepo.FirstOrDefault(s => s.Student.DepartmentBranch.DepartmentId == departmentId && s.Student.DepartmentBranch.BranchId == branchId && s.FinalGradePercent == null);
                return unFinishedCourse != null ? -1 : 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CanRemoveDepartmentFromBranch - DepartmentService");
                return 0;
            }
        }


    }
}
