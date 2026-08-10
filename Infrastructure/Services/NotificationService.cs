using Application.Services.Interfaces;

namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailService emailService;

    public NotificationService(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public async Task SendNotificationAsync(string to, string subject, string body, bool isHttp)
    {
        await this.emailService.SendEmailAsync(to, subject, body, isHttp);
    }
}
