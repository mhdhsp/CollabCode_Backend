using System.ComponentModel.DataAnnotations;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class NewUserResDto
    {
        public string? UserName { get; set; }
        public int? Id { get; set; }
        public string? Token { get; set; } = null;
    }
}
