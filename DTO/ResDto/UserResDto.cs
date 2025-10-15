using System.ComponentModel.DataAnnotations;

namespace CollabCode.DTO.ResDto
{
    public class UserResDto
    {
        public string? UserName { get; set; }
        public string? Token { get; set; } = null;
    }
}
