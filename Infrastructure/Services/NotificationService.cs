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

    public async Task SendLoginNotificationAsync(string username)
    {
        var subject = "Login Notification";
        var body = $"User {username} has logged in.";
        await this.emailService.SendEmailAsync(username, subject, body, false);
    }
}
