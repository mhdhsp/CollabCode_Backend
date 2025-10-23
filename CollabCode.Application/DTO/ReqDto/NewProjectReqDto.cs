namespace CollabCode.CollabCode.Application.DTO.ReqDto
{
    public class NewProjectReqDto
    {
        public string? ProjectName { get; set; }

        public bool IsPublic { get; set; } = true;

        public string? PassWordHash { get; set; }

    }
}
