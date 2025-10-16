namespace CollabCode.Common.DTO.ReqDto
{
    public class NewRoomReqDto
    {
        public string? RoomName { get; set; }

        public bool IsPublic { get; set; } = true;

        public string Language { get; set; } = "JavaScript";
        public string? PassWordHash { get; set; }

    }
}
