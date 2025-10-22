using CollabCode.CollabCode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class NewUserReqDto
    {
       
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email is required ! ")]
        [EmailAddress(ErrorMessage = "Invalid email format ! ")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required ! ")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 20 characters.")]
        public string? PassWord { get; set; }

    }
}
