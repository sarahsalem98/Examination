using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IBranchService
    {
        public List<BranchMV> GetByStatus(int status);
        public int Add(BranchMV branch, List<int> DepIds);

        public BranchMV GetById(int id);

        public int Update(BranchMV branch);

        public int ChangeStatus(int id, int status);
        public PaginatedData<BranchMV> GetAllPaginated(BranchSearchMV branchSearch, int pageSize = 10, int page = 1);
        public List<String> GetDistinctBranchLocations();
    }

}
