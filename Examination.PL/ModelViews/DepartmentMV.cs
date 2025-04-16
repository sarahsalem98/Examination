namespace Examination.PL.ModelViews
{
    public class DepartmentMV
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Capacity { get; set; }

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<string> BranchNames { get; set; }

    }

    public class DepartmentBranchMV
    {
        public int Id { get; set; }

        public int BranchId { get; set; }

        public int DepartmentId { get; set; }

        public DepartmentMV Department { get; set; } = null!;
        public BranchMV Branch { get; set; } = null!;
    }

    public class DepartmentSearchMV
    {
        public string? Name { get; set; }
        public int? Status { get; set; }

        public int? BranchId { get; set; }

    }
}
