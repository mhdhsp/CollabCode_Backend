using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class ChatResDto
    {
        public int senderId { get; set; }
        public int ProjectId { get; set; }
        public string? SenderName { get; set; }
        public string? Content { get; set; }
        public DateTime Time { get; set; }


    }
}
