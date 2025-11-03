namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class SaveFileReqDto
    {
       public  int FileId { get; set; }
        public int ProjectId { get; set; }
       public  string? Content { get; set; }
    }
}
