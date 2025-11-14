using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CollabCode.CollabCode.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] ContactMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input" });

            try
            {
                await _emailService.SendContactEmailAsync(dto);
                return Ok(new { message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to send message.",
                    detail = ex.Message
                });
            }
        }
    }
}
