using System.ComponentModel.DataAnnotations;

namespace CollabCode.Model
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PassWord { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
