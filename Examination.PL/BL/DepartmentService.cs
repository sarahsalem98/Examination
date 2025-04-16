using AutoMapper;
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
                var departments = _unitOfWork.DepartmentRepo.GetAll(d => d.DepartmentBranches.Select(b => b.BranchId).Contains(branchId));
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
                        (departmentSearch.Status == null || d.Status == departmentSearch.Status) &&
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
                    BranchIds = d.DepartmentBranches.Select(b => b.BranchId).ToList(),
                    BranchNames = d.DepartmentBranches.Select(b => b.Branch?.Name ?? "Unknown").ToList()
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


        //public int Add(DepartmentMV department)
        //{
        //    int result = 0;
        //    try
        //    {

        //        var existing = _unitOfWork.DepartmentRepo.FirstOrDefault(d => d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

        //        if (existing != null)
        //        {
        //            return -1;
        //        }

        //        var newDepartment = _mapper.Map<Department>(department);
        //        newDepartment.CreatedAt = DateTime.Now;
        //        newDepartment.CreatedBy = 1; 
        //        _unitOfWork.DepartmentRepo.Insert(newDepartment);
        //        return _unitOfWork.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while adding new department");
        //        return 0;
        //    }
        //}


        //public int Add(DepartmentMV department)
        //{
        //    try
        //    {
        //        // Check if a department with the same name already exists (case-insensitive)
        //        var existing = _unitOfWork.DepartmentRepo
        //            .FirstOrDefault(d => d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

        //        if (existing != null)
        //        {
        //            return -1; // duplicate
        //        }

        //        // Map DepartmentMV to Department entity
        //        var newDepartment = _mapper.Map<Department>(department);

        //        // Set metadata
        //        newDepartment.CreatedAt = DateTime.Now;
        //        newDepartment.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
        //        newDepartment.Status = (int)Status.Active;

        //        // Validate that at least one BranchId is provided
        //        if (department.BranchIds == null || !department.BranchIds.Any())
        //        {
        //            throw new Exception("At least one branch must be assigned to the department.");
        //        }

        //        // Assign selected branches to the department
        //        newDepartment.DepartmentBranches = department.BranchIds
        //            .Select(branchId => new DepartmentBranch
        //            {
        //                BranchId = branchId
        //            }).ToList();

        //        // Insert and save
        //        _unitOfWork.DepartmentRepo.Insert(newDepartment);
        //        return _unitOfWork.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while adding new department");
        //        return 0;
        //    }
        //}

        //public int Add(DepartmentMV department)
        //{
        //    int result = 0;

        //    try
        //    {
        //        // Check for duplicates
        //        var existing = _unitOfWork.DepartmentRepo
        //            .FirstOrDefault(d => d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

        //        if (existing != null)
        //            return -1;

        //        // Map ViewModel to Entity
        //        var newDepartment = _mapper.Map<Department>(department);
        //        newDepartment.CreatedAt = DateTime.Now;
        //        newDepartment.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
        //        newDepartment.Status = (int)Status.Active;

        //        // Validate Branches are selected
        //        if (department.BranchIds == null || !department.BranchIds.Any())
        //            throw new Exception("At least one branch must be selected.");

        //        // Set DepartmentBranches from selected IDs
        //        newDepartment.DepartmentBranches = department.BranchIds.Select(branchId => new DepartmentBranch
        //        {
        //            BranchId = branchId
        //        }).ToList();

        //        _unitOfWork.DepartmentRepo.Insert(newDepartment);
        //        return _unitOfWork.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error while adding department");
        //        return 0;
        //    }
        //}





        public int Add(DepartmentMV department)
        {
            int result = 0;
            try
            {
                // Check if a department with the same name already exists
                var existing = _unitOfWork.DepartmentRepo
                    .FirstOrDefault(d => d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

                if (existing != null)
                {
                    result = -1; // Department already exists
                }
                else
                {
                    // Ensure branches are selected
                    if (department.BranchIds == null || !department.BranchIds.Any())
                    {
                        throw new Exception("At least one branch must be selected.");
                    }

                    // Map view model to entity
                    var newDepartment = _mapper.Map<Department>(department);
                    newDepartment.CreatedAt = DateTime.Now;
                    newDepartment.CreatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);
                    newDepartment.Status = (int)Status.Active;

                    // Link selected branches
                    newDepartment.DepartmentBranches = department.BranchIds.Select(branchId => new DepartmentBranch
                    {
                        BranchId = branchId
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
                        result = -1; // Duplicate name
                    }
                    else
                    {
                        // Check branch selection
                        if (department.BranchIds == null || !department.BranchIds.Any())
                        {
                            throw new Exception("At least one branch must be selected.");
                        }

                        // Preserve created fields
                        department.CreatedAt = departmentExist.CreatedAt;
                        department.CreatedBy = departmentExist.CreatedBy;

                        // Map updated values
                        _mapper.Map(department, departmentExist);

                        // Update timestamps
                        departmentExist.UpdatedAt = DateTime.Now;
                        departmentExist.UpdatedBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value);

                        // Clear and set new DepartmentBranches
                        departmentExist.DepartmentBranches.Clear();
                        foreach (var branchId in department.BranchIds)
                        {
                            departmentExist.DepartmentBranches.Add(new DepartmentBranch
                            {
                                BranchId = branchId,
                                DepartmentId = department.Id
                            });
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







        //public int Update(DepartmentMV department)
        //{
        //    try
        //    {
        //        var existing = _unitOfWork.DepartmentRepo
        //            .FirstOrDefault(d => d.Id != department.Id && d.Name.Trim().ToLower() == department.Name.Trim().ToLower());
        //        if (existing != null)
        //        {
        //            return -1;
        //        }
        //        var updatedDepartment = _mapper.Map<Department>(department);
        //        updatedDepartment.UpdatedAt = DateTime.Now;
        //        updatedDepartment.UpdatedBy = 1;
        //        _unitOfWork.DepartmentRepo.Update(updatedDepartment);
        //        return _unitOfWork.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while updating department");
        //        return 0;
        //    }
        //}




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

    }
}
