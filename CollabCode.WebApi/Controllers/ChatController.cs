using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Services;
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
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;
        public ChatController(IChatService service)
        {
            _service = service;
        }

        [HttpGet("GetAllMsg/{projectId}")]
        public async Task<IActionResult> GetAllMsg(int projectId, [FromQuery] int limit)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("user not found ,login required");
            int userId = Convert.ToInt32(user);

            var res =await  _service.GetAllMsg(projectId,userId,limit);
            if (res == null)
                return NotFound(new ApiResponse<string> { Message = "msg not found" });
            return Ok(new ApiResponse<List<ChatResDto>> { Message = "Messages Found Succesfuly", Data = res });
        }

        [HttpPost("AddMsg")]
        public async Task<IActionResult> AddMsg(ChatReqDto newMsg)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("user not found ,login required");
            int userId = Convert.ToInt32(user);

            var res =await  _service.AddMsg(newMsg,userId);
            if (res == null)
                return NotFound(new ApiResponse<string> { Message = "msg not found" });
            return Ok(new ApiResponse<Chat> { Message = "Messages sent Succesfuly", Data = res });
        }


    }
}
