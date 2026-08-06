using Application.Services.Interfaces;

namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailService emailService;

    public NotificationService(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public Task SendWelcomeNotificationAsync(string to, string subject, string body, bool isHttp)
    {
        return this.emailService.SendEmailAsync(to, subject, body, isHttp);
    }
}
