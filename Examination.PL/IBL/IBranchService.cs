using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IBranchService
    {
        public List<BranchMV> GetByStatus(int status);
    }
}
