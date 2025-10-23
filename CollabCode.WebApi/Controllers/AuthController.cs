//using AutoMapper;
//using CollabCode.CollabCode.WebApi.Common;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using CollabCode.CollabCode.Application.DTO.ReqDto;
//using CollabCode.CollabCode.Application.DTO.ResDto;
//using CollabCode.CollabCode.Domain.Entities;
//using CollabCode.CollabCode.Application.Services;
//using CollabCode.CollabCode.Application.Interfaces.Services;

//namespace CollabCode.CollabCode.WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly IAuthService _service;
//        private readonly ILogger<AuthController> _logger;

//        public AuthController(IAuthService service, ILogger<AuthController> logger)
//        {
//            _service = service;
//            _logger = logger;

//        }

//        [HttpPost("Register")]
//        public async Task<ActionResult> Register(NewUserReqDto newUser)
//        {
//            var res = await _service.Register(newUser);
//            _logger.LogInformation($"{newUser.UserName} registerd succefully");
//            return Ok(new ApiResponse<NewUserResDto> { Message = "User added ", Data = res });
//        }

//        [HttpPost("Login")]
//        public async Task<ActionResult> Login(LoginReqDto ReqDto)
//        {
//            var res = await _service.Login(ReqDto);
//            _logger.LogInformation($"{ReqDto.UserName} Loginned succesfully");
//            return Ok(new ApiResponse<NewUserResDto> { Message = "Loginned succesfully", Data = res });
//        }


//    }
//}
