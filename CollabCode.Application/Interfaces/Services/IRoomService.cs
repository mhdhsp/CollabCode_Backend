using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<NewRoomResDto> CreateNewRoom(NewRoomReqDto reqDto, int userId);
        Task<NewRoomResDto> JoinRoom(RoomJoinReqDto reqDto, int userId);

        Task<RoomResDto> EnterRoom(int Roomid, int userId);
    }
}
