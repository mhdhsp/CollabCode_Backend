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
    public class FileController : ControllerBase
    {
        private readonly IFileService _service;
        public FileController(IFileService service)
        {
            _service = service;
        }
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewFile(NewFileReqDto item)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.CreateFile(item, userId);
            if (res == null)
                return NotFound(new ApiResponse<Object> { Message = "Couldnt create file" });
            return Ok(new ApiResponse<NewFileResDto> { Message = "Succefully created file", Data = res });
        }


        [HttpDelete("Delete/{fileId}")]
        public async Task<ActionResult> CreateFile(int fileId)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.DeleteFile(fileId, userId);
            if (res == false)
                return NotFound(new ApiResponse<Object> { Message = "Couldnt delete file" });
            return Ok(new ApiResponse<bool> { Message = "Succefully deleted file", Data = res });

        }
        [HttpPatch("Save")]
        public async Task<ActionResult> SaveFile(SaveFileReqDto dto)
        {
            Console.WriteLine($"save file {dto}");
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.SaveFile(dto, userId);
            if (res ==null)
                return NotFound(new ApiResponse<Object> { Message = "Couldnt save  file" });
            return Ok(new ApiResponse<ProjectFile> { Message = "Succefully saved   file", Data = res });
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateFile(FileUpdateReqDto dto )
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.UpdateFile(dto, userId);
            if (res == false)
                return NotFound(new ApiResponse<Object> { Message = "Couldnt Update file" });
            return Ok(new ApiResponse<bool> { Message = "Succefully updated  file", Data = res });
        }

        [HttpPatch("Assign")]
        public async Task<ActionResult> AssignToUser(FileAssignReqDto dto)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var res = await _service.Assign(dto, userId);
            if (res == null)
                return BadRequest(new ApiResponse<string> { Message = "File cannot assigned" });
            return Ok(new ApiResponse<ProjectFile> { Message = "File Assigned ", Data = res });
        }

        [HttpPatch("UnAssign/{id}")]
        public async Task<ActionResult> UnAssign(int id)
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);


            var res = await _service.UnAssign(id, userId);
            if (res == null)
                return BadRequest(new ApiResponse<string> { Message = "File cannot Unassigned" });
            return Ok(new ApiResponse<ProjectFile> { Message = "File UnAssigned ", Data = res });
        }
    }
}
