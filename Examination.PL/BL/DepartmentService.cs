using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using System.Drawing.Printing;

namespace Examination.PL.BL
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
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

        //public PaginatedData<DepartmentMV> GetAllPaginated(DepartmentSearchMV Departmentsearch, int pageSize, int page)
        //{
        //    try
        //    {
        //        List<DepartmentMV> departmentMVs = new List<DepartmentMV>();
        //        List<Department> data = _unitOfWork.DepartmentRepo.GetAll(
        //    d =>
        //        (string.IsNullOrEmpty(Departmentsearch.Name) || d.Name.ToLower().Trim().Contains(Departmentsearch.Name.ToLower().Trim())) &&
        //        (Departmentsearch.Status == null || d.Status == Departmentsearch.Status) &&
        //        (Departmentsearch.BranchId == null || d.DepartmentBranches.Any(b => b.BranchId == Departmentsearch.BranchId)),
        //           "DepartmentBranches"
        //         )
        //         .OrderByDescending(d => d.CreatedAt)
        //         .ToList();

        //        departmentMVs = _mapper.Map<List<DepartmentMV>>(data);
        //        int totalCount = departmentMVs.Count;

        //        if (totalCount > 0)
        //        {
        //            departmentMVs = departmentMVs
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize).ToList();

        //        }

        //        return new PaginatedData<DepartmentMV>
        //        {
        //            Items = departmentMVs,
        //            TotalCount = totalCount,
        //            PageSize = pageSize,
        //            CurrentPage = page
        //        };


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in GetAllPaginated DepartmentService");
        //        return new PaginatedData<DepartmentMV>();
        //    }
        //}



        public PaginatedData<DepartmentMV> GetAllPaginated(DepartmentSearchMV search, int pageSize = 10, int page = 1)
        {
            try
            {
                var data = _unitOfWork.DepartmentRepo.GetAll(
                    d =>
                        (string.IsNullOrEmpty(search.Name) || d.Name.ToLower().Trim().Contains(search.Name.ToLower().Trim())) &&
                        (search.Status == null || d.Status == search.Status) &&
                        (search.BranchId == null || d.DepartmentBranches.Any(b => b.BranchId == search.BranchId)),
                     "DepartmentBranches.Branch" 
                )
                .OrderByDescending(d => d.CreatedAt)
                .ToList();

                var mapped = _mapper.Map<List<DepartmentMV>>(data);

                int totalCount = mapped.Count;

                var paged = mapped
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PaginatedData<DepartmentMV>
                {
                    Items = paged,
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page
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
            try
            {
                //check
                var existing = _unitOfWork.DepartmentRepo
                    .FirstOrDefault(d => d.Name.Trim().ToLower() == department.Name.Trim().ToLower());

                if (existing != null)
                {
                    return -1; //already exists
                }

                var newDepartment = _mapper.Map<Department>(department);
                newDepartment.CreatedAt = DateTime.Now;
                newDepartment.CreatedBy = 1; // Replace with current user ID

                _unitOfWork.DepartmentRepo.Insert(newDepartment);
                return _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new department");
                return 0;
            }
        }


        public int Update(DepartmentMV department)
        {
            try
            {
                var existing = _unitOfWork.DepartmentRepo
                    .FirstOrDefault(d => d.Id != department.Id && d.Name.Trim().ToLower() == department.Name.Trim().ToLower());
                if (existing != null)
                {
                    return -1; //already exists
                }
                var updatedDepartment = _mapper.Map<Department>(department);
                updatedDepartment.UpdatedAt = DateTime.Now;
                updatedDepartment.UpdatedBy = 1; // Replace with current user ID
                _unitOfWork.DepartmentRepo.Update(updatedDepartment);
                return _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating department");
                return 0;
            }
        }




    }
}
