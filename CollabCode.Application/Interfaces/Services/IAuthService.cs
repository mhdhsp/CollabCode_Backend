using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserResDto?> Register(User newUser);
        Task<UserResDto> Login(LoginReqDto ReqDto);
    }
}
