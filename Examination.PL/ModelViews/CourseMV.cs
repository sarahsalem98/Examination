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

    public List<int> DepartmentsIds { get; set; }
    public List<InstructorCourseMV>? InstructorCourses { get; set; } = null!;
    public List<CourseDepartmentMV>? CourseDepartments { get; set; } = null!;


}

public class CourseSearchMV
{

    public string? Name { get; set; }
    public int? Status { get; set; }
    public int? DepartmentId { get; set; }
    public int? BranchId { get; set; }
    public int? TrackType { get; set; }
}
public class CourseDepartmentMV
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int DepartmentId { get; set; }
}
public class InstructorCourseMV
{
    public int? Id { get; set; }

    public int? InstructorId { get; set; }

    public int CourseId { get; set; }

    public int? DepartmentBranchId { get; set; }

    public CourseMV Course { get; set; } = null!;

    public DepartmentBranchMV DepartmentBranch { get; set; } = null!;

    public InstructorMV Instructor { get; set; } = null!;
}

