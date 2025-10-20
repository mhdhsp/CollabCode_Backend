using System.Collections.Generic;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class UserRoomsDto
    {
        public string? UserName { get; set; }
        public List<RoomDto> OwnedRooms { get; set; } = new();
        public List<RoomDto> JoinedRooms { get; set; } = new();
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public string? RoomName { get; set; }
        public string? JoinCode { get; set; }
    }
}
