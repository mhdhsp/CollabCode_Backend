using CollabCode.CollabCode.Domain.Entities;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class RoomResDto
    {
        public string? RoomName { get; set; }

        public int OwnerId { get; set; }
        public bool IsPublic { get; set; } = false;

        public string Language { get; set; } = "JavaScript";

        public string? CurrentCode { get; set; } = "//Start coding now...";
        public bool IsActive { get; set; } = true;

        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
    }
    public class MemberDto
    {
        public int id { get; set; }
        public string? UserName { get; set; }
        public bool isOwner { get; set; } = false;

    }

}
