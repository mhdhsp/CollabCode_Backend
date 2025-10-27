using CollabCode.CollabCode.Application.DTO.ResDto;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserProjectsDto?> GetAllUserProjects(int userid);
        Task<bool> DeleteUserAsync(int id);
    }
}
