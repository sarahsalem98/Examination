namespace Examination.PL.ModelViews;

public class CourseMV
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Hours { get; set; }

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
