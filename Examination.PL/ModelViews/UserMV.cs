using System.ComponentModel.DataAnnotations;

namespace Examination.PL.ModelViews
{
    public class AccountLoginMV
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class UserMV
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public int? Age { get; set; }

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<UserTypeMV>? UserTypes { get; set; } = new List<UserTypeMV>();

    }

    public class UserTypeMV
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = null!;
    }
}
