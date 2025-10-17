using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Application.Services;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.WebApi.Common;
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
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res =await  _service.CreateNewRoom(ReqDto,userId);
             return Ok(new ApiResponse<NewRoomResDto> { Message = "room created", Data = res });
        }

        [HttpPost("JoinRoom")]
        public async Task<ActionResult> JoinRoom(RoomJoinReqDto ReqDto)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.JoinRoom(ReqDto,userId);
            return Ok(new ApiResponse<NewRoomResDto> { Message = "joined success fully", Data = res });
        }

        [HttpGet("EnterRoom/{RoomId}")]
        public async Task<ActionResult> EnterRoom(int RoomId)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res=await  _service.EnterRoom(RoomId,userId);
            if (res == null)
                return NotFound(new ApiResponse<string> {Message= "Room not found" });
            return Ok(new ApiResponse<RoomResDto> { Message = "Room found ", Data = res });
        }

    }
}
