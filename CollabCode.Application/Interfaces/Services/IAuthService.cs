using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<NewUserResDto?> Register(User newUser);
        Task<NewUserResDto> Login(LoginReqDto ReqDto);
    }
}
