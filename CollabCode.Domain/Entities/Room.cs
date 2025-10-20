using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string? RoomName { get; set; }
        public string? JoinCode { get; set; }

        public int OwnerId { get; set; }
        public bool IsPublic { get; set; } = false;
        public string? PassWordHash { get; set; } 

        public string Language { get; set; } = "JavaScript";
        
        public string? CurrentCode { get; set; } = "//Start coding now...";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        

        [JsonIgnore]
        public User? Owner { get; set; }

        [JsonIgnore]
        public List<RoomMember> Members { get; set; } = new List<RoomMember>();
        
    }
}
