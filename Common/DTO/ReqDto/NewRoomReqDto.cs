namespace CollabCode.Common.DTO.ReqDto
{
    public class NewRoomReqDto
    {
        public string? RoomName { get; set; }

        public bool IsPublic { get; set; } = false;

        public string Language { get; set; } = "JavaScript";

    }
}
