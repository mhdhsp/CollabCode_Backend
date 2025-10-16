using CollabCode.Common.DTO.ReqDto;
using CollabCode.Common.DTO.ResDto;
using CollabCode.Common.Model;
using CollabCode.Models;
using CollabCode.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollabCode.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

    }
}
