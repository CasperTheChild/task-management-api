using Application.Services.Interfaces;

namespace Infrastructure.Services;

public class SendGridEmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, bool isHtml)
    {
        throw new NotImplementedException();
    }
}
