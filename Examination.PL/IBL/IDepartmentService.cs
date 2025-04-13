using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IDepartmentService
    {
        public List<DepartmentMV> GetByStatus(int status);
        public List<DepartmentMV> GetByBranch(int branchId);

        //List<DepartmentMV> GetAll();
        DepartmentMV GetById(int id);
        PaginatedData<DepartmentMV> GetAllPaginated(DepartmentSearchMV search, int pageSize, int page);

        int Add(DepartmentMV department);
     //   int Update(DepartmentMV department);
      //  int Delete(int id);
    }
}
