using MimeKit;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MimeMessage message);
    }
}
