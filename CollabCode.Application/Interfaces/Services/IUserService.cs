using CollabCode.CollabCode.Application.DTO.ResDto;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserRoomsDto?> GetAllUserRooms(int userid);
    }
}
