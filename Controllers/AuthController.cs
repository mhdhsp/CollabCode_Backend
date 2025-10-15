using AutoMapper;
using CollabCode.DTO.ReqDto;
using CollabCode.DTO.ResDto;
using CollabCode.Exceptions;
using CollabCode.Model;
using CollabCode.Models;
using CollabCode.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollabCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger<AuthController> _logger;
     
       public AuthController(IAuthService service,ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
            
        }

        [HttpPost("Resgister")]
        public async Task<ActionResult> Register(UserModel newUser)
        {
            
            
                var res = await _service.Register(newUser);
                _logger.LogInformation($"{newUser.UserName} registerd succefully");
                return Ok(new ApiResponse<UserResDto> { Message = "User added ", Data = res });
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginReqDto ReqDto)
        {
            var res = await _service.Login(ReqDto);
            _logger.LogInformation($"{ReqDto.UserName} Loginned succesfully");
            return Ok(new ApiResponse<UserResDto> { Message = "Loginned succesfully", Data = res });
        }
        

    }
}
