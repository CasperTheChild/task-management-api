using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SendGridEmailService : IEmailService
{
    private readonly ILogger<SendGridEmailService> logger;

    public SendGridEmailService(ILogger<SendGridEmailService> logger)
    {
        this.logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
    {
        await Task.Delay(5000);

        this.logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
    }
}
