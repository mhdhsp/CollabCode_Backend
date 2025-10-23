using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class FileUpdateReqDto
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? Content { get; set; } 

        public int? AssignedTo { get; set; } 
        public DateTime? AssignedAt { get; set; }

        public int ProjectId { get; set; }
    }
}
