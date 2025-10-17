using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Services;
using CollabCode.Common.Model;
using CollabCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollabCode.CollabCode.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;
        public RoomController(IRoomService service)
        {
            _service = service;
        }
        [HttpPost("NewRoom")]
        public async Task<ActionResult> CreateNewRoom(NewRoomReqDto ReqDto)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                return Unauthorized("User not found in request context");
            int userId= userId = Convert.ToInt32(user);

            var res =await  _service.CreateNewRoom(ReqDto,userId);
             return Ok(new ApiResponse<RoomResDto> { Message = "room created", Data = res });
        }

        [HttpPost("JoinRoom")]
        public async Task<ActionResult> JoinRoom(RoomJoinReqDto ReqDto)
        {

        }

    }
}
