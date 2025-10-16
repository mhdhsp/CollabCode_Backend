using Microsoft.AspNetCore.Http.HttpResults;

namespace CollabCode.Common.Model
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string? RoomName { get; set; }
        public int JoinCode { get; set; }

        public int OwnerId { get; set; }
        public bool IsPublic { get; set; } = false;

        public string Language { get; set; } = "JavaScript";
        
        public string? CurrentCode { get; set; } = "//Start coding now...";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
    }
}
