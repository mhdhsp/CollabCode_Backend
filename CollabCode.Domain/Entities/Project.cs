using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Domain.Entities
{
    public class Project:BaseEntity
    {
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? JoinCode { get; set; }

        public int OwnerId { get; set; }
        public bool IsPublic { get; set; } = false;
        public string? PassWordHash { get; set; } 

        public string Language { get; set; } = "JavaScript";
        
        [JsonIgnore]
        public User? Owner { get; set; }
        [JsonIgnore]
        public List<ProjectFile> Files { get; set; } = new List<ProjectFile>();
        [JsonIgnore]
        public List<MemberShip> Members { get; set; }= new List<MemberShip>();

    }
}
