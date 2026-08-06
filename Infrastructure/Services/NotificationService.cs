using Application.Services.Interfaces;

namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly SendGridEmailService emailService;

    public NotificationService(SendGridEmailService emailService)
    {
        this.emailService = emailService;
    }

    public Task SendWelcomeNotificationAsync(string to, string subject, string body, bool isHttp)
    {
        return this.emailService.SendEmailAsync(to, subject, body, isHttp);
    }
}
