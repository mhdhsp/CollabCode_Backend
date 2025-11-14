using CollabCode.CollabCode.Application.DTO.ReqDto;

namespace CollabCode.CollabCode.Application.Services
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(ContactMessageDto dto);
    }
} 
