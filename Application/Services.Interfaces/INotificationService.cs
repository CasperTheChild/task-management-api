namespace Application.Services.Interfaces;

public interface INotificationService
{
    public Task SendWelcomeNotificationAsync(string to, string subject, string body, bool isHttp);
}
