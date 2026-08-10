using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SendGridEmailService : IEmailService
{
    private readonly ILogger logger;

    public SendGridEmailService(ILogger<SendGridEmailService> logger)
    {
        this.logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body, bool isHtml)
    {
        this.logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
        return Task.CompletedTask;
    }
}
