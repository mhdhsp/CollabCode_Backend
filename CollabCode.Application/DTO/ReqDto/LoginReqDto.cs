using System.ComponentModel.DataAnnotations;

namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class LoginReqDto
    {
        [Required(ErrorMessage = "UserName is required ! ")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters.")]
        public string? UserName { get; set; }


       
        [Required(ErrorMessage = "Password is required ! ")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 20 characters.")]
        public string? PassWord { get; set; }
    }
}
