using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabCode.CollabCode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }


        [HttpGet("Projects")]
        public async Task<IActionResult> GetUserProjects()
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var result = await _service.GetAllUserProjects(userId);
            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _service.DeleteUserAsync(id);

            if (!deleted)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(new { message = $"User with ID {id} has been deleted successfully." });
        }

    }
}
