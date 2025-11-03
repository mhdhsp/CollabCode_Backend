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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service)
        {
            _service = service;

        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(NewProjectReqDto ReqDto)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.CreateNewProject(ReqDto, userId);
            return Ok(new ApiResponse<NewProjectResDto> { Message = "project created", Data = res });
        }

        [HttpPost("Join")]
        public async Task<ActionResult> Join(ProjectJoinReqDto ReqDto)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.JoinProject(ReqDto, userId);
            return Ok(new ApiResponse<NewProjectResDto> { Message = "joined success fully", Data = res });
        }

        [HttpGet("Enter/{ProjectId}")]
        public async Task<ActionResult> Enter([FromRoute] int ProjectId)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.EnterProject(ProjectId, userId);
            if (res == null)
                return NotFound(new ApiResponse<string> { Message = "project not found" });
            return Ok(new ApiResponse<ProjectResDto> { Message = "project found ", Data = res });
        }

        [HttpGet("Leave/{ProjectId}")]
        public async Task<ActionResult> Leave(int ProjectId)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.LeaveProject(userId, ProjectId);
            if (res == true)
                return Ok(new ApiResponse<bool> { Message = "Left the project succesfully" });
            return BadRequest(new ApiResponse<bool> { Message = "Somthing went wrong" });
        }

        [HttpDelete("destroy/{ProjectId}")]
        public async Task<ActionResult> Destroy(int ProjectId)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found, login required");

            int userId = Convert.ToInt32(user);

            var res = await _service.DestroyProject(userId, ProjectId);

            if (res)
                return Ok(new ApiResponse<bool> { Message = "Project deleted successfully", Data = true });

            return BadRequest(new ApiResponse<bool> { Message = "Something went wrong", Data = false });
        }

        [HttpDelete("Member/{projectId}/{memberId}")]
        public async Task<ActionResult> RemoveMember(int projectId, int memberId)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new UnauthorizedAccessException("User id not found, login required");

            int userId = Convert.ToInt32(user);

            await _service.RemoveMember(userId, projectId, memberId);
            return Ok(new ApiResponse<bool> { Message = "Member removed successfully", Data = true });
        }


    }
}
