namespace Examination.PL.ModelViews
{
    public class StudentMV
    {
        public int Id { get; set; }

        public int DepartmentBranchId { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public int? TrackType { get; set; }

        public int UserId { get; set; }

        public UserMV? UserMV { get; set; }
    }
}
