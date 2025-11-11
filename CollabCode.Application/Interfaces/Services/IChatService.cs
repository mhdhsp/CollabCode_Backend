using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<ChatResDto>> GetAllMsg(int projectId, int userId, int limit = 10);
        Task<Chat> AddMsg(ChatReqDto newMsg, int userId);
    }
}
