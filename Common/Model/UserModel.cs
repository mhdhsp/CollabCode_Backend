using System.ComponentModel.DataAnnotations;

namespace CollabCode.Common.Model
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="UserName is required ! ")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email is required ! ")]
        [EmailAddress(ErrorMessage = "Invalid email format ! ")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required ! ")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 20 characters.")]
        public string? PassWord { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }

    }
}
