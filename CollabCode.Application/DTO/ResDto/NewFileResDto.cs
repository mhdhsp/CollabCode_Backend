using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class NewFileResDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int? AssignedTo { get; set; } // Null = unlocked

        public int ProjectId { get; set; }

    }
}
