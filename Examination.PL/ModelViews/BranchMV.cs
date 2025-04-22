namespace Examination.PL.ModelViews
{
    public class BranchMV
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class BranchSearchMV
    {
        public string? Name { get; set; }
        public string Location { get; set; }
        public int? Status { get; set; }
    }
}
