using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IDepartmentService
    {
        public List<DepartmentMV> GetByStatus(int status);
        public List<DepartmentMV> GetByBranch(int branchId);
    }
}
