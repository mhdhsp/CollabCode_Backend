using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Repositories
{
    public interface IRoomRepo
    {
        Task<RoomResDto?> GetByIdAsync(int id);
    }
}
