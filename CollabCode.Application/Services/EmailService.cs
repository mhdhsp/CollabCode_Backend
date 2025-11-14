using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace CollabCode.CollabCode.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendContactEmailAsync(ContactMessageDto dto)
        {
            var smtp = new SmtpClient(_config["SMTP:Host"])
            {
                Port = int.Parse(_config["SMTP:Port"]),
                Credentials = new NetworkCredential(
                    _config["SMTP:User"],
                    _config["SMTP:Pass"]
                ),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_config["SMTP:From"]),
                Subject = $"New Contact Message from {dto.Name}",
                Body =
                    $"Name: {dto.Name}\n" +
                    $"Email: {dto.Email}\n\n" +
                    $"Message:\n{dto.Message}",
                IsBodyHtml = false
            };

            // Developer email
            mail.To.Add(_config["SMTP:DeveloperEmail"]);

            // Reply-To user
            mail.ReplyToList.Add(new MailAddress(dto.Email));

            await smtp.SendMailAsync(mail);
        }
    }
}
