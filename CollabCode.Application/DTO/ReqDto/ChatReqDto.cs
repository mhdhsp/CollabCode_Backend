using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class ChatReqDto
    {
        public int ProjectId { get; set; }
        public string? Content { get; set; }

    }
}
