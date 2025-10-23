//using CollabCode.CollabCode.Application.DTO.ReqDto;
//using CollabCode.CollabCode.Application.DTO.ResDto;
//using CollabCode.CollabCode.Application.Exceptions;
//using CollabCode.CollabCode.Application.Interfaces.Services;
//using CollabCode.CollabCode.Application.Services;
//using CollabCode.CollabCode.Domain.Entities;
//using CollabCode.CollabCode.WebApi.Common;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace CollabCode.CollabCode.WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class ProjectController : ControllerBase
//    {
//        private readonly IProjectService _service;
//        public ProjectController(IProjectService service)
//        {
//            _service = service;

//        }
//        [HttpPost("Create")]
//        public async Task<ActionResult> Create(NewProjectReqDto ReqDto)
//        {
//            var user = HttpContext.Items["UserId"]?.ToString();
//            if (user == null)
//                throw new NotFoundException("User id not found,login required");
//            int userId = Convert.ToInt32(user);

//            var res = await _service.CreateNewProject(ReqDto, userId);
//            return Ok(new ApiResponse<NewRoomResDto> { Message = "room created", Data = res });
//        }

//        [HttpPost("Join")]
//        public async Task<ActionResult> Join(RoomJoinReqDto ReqDto)
//        {
//            var user = HttpContext.Items["UserId"]?.ToString();
//            if (user == null)
//                throw new NotFoundException("User id not found,login required");
//            int userId = Convert.ToInt32(user);

//            var res = await _service.JoinRoom(ReqDto, userId);
//            return Ok(new ApiResponse<NewRoomResDto> { Message = "joined success fully", Data = res });
//        }

//        [HttpGet("Enter/{RoomId}")]
//        public async Task<ActionResult> Enter([FromRoute] int RoomId)
//        {
//            var user = HttpContext.Items["UserId"]?.ToString();
//            if (user == null)
//                throw new NotFoundException("User id not found,login required");
//            int userId = Convert.ToInt32(user);

//            var res = await _service.EnterRoom(RoomId, userId);
//            if (res == null)
//                return NotFound(new ApiResponse<string> { Message = "Room not found" });
//            return Ok(new ApiResponse<RoomResDto> { Message = "Room found ", Data = res });
//        }

//        [HttpGet("Leave/{roomId}")]
//        public async Task<ActionResult> Leave(int roomId)
//        {
//            var user = HttpContext.Items["UserId"]?.ToString();
//            if (user == null)
//                throw new NotFoundException("User id not found,login required");
//            int userId = Convert.ToInt32(user);

//            var res = await _service.LeaveRoom(userId, roomId);
//            if (res == true)
//                return Ok(new ApiResponse<bool> { Message = "Left the room succesfully" });
//            return BadRequest(new ApiResponse<bool> { Message = "Somthing went wrong" });
//        }

//        [HttpDelete("destroy/{roomId}")]
//        public async Task<ActionResult> Destroy(int roomId)
//        {
//            var user = HttpContext.Items["UserId"]?.ToString();
//            if (user == null)
//                throw new NotFoundException("User id not found, login required");

//            int userId = Convert.ToInt32(user);

//            var res = await _service.DestroyRoom(userId, roomId);

//            if (res)
//                return Ok(new ApiResponse<bool> { Message = "Room deleted successfully", Data = true });

//            return BadRequest(new ApiResponse<bool> { Message = "Something went wrong", Data = false });
//        }


//    }
//}
