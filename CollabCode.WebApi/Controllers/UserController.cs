using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CollabCode.CollabCode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetUserRooms()
        {
            var user = HttpContext.Items["UserId"]?.ToString();
            if (user == null)
                throw new NotFoundException("User id not found,login required");
            int userId = Convert.ToInt32(user);

            var result = await _service.GetAllUserRooms(userId);
            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }
    }
}
