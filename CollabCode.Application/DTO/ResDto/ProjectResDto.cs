using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Domain.Enums;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class ProjectResDto
    {
        public string? ProjectName { get; set; }

        public int OwnerId { get; set; }
        public string? joinCode { get; set; }

        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
        public List<FileDto> Files { get; set; } = new List<FileDto>();
    }
    public class MemberDto
    {
        public int id { get; set; }
        public string? UserName { get; set; }

    }
    public class FileDto
    {
        public int Id { get; set; }
        public string? FileName { get; set; } 
        public string? Content { get; set; } 

        public int? AssignedTo { get; set; } 

        public int ProjectId { get; set; }
        public string? Status { get; set; }
    }

}
