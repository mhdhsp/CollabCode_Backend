using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class NewFileReqDto
    {
        public string FileName { get; set; } = string.Empty;

        public int ProjectId { get; set; }
    }
}
