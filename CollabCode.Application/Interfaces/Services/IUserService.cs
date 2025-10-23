using CollabCode.CollabCode.Application.DTO.ResDto;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserProjectsDto?> GetAllUserRooms(int userid);
    }
}
