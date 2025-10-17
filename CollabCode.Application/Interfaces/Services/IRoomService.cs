using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<RoomResDto> CreateNewRoom(NewRoomReqDto reqDto, int userId);
        Task<RoomResDto> JoinRoom(RoomJoinReqDto reqDto, int userId);

       Task <RoomResDto>
    }
}
