using CollabCode.Common.Model;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class RoomResDto
    {
        public string? RoomName { get; set; }
        public string? JoinCode { get; set; }

        public int OwnerId { get; set; }
        public bool IsPublic { get; set; } = false;

        public string Language { get; set; } = "JavaScript";

        public string? CurrentCode { get; set; } = "//Start coding now...";

    }
}
