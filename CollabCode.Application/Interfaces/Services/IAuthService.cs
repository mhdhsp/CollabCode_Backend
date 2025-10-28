using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<NewUserResDto?> Register(NewUserReqDto user);
        Task<NewUserResDto> Login(LoginReqDto ReqDto);
        Task<bool> Logout(int userId);
    }
}
