namespace Application.Services.Interfaces;

public interface INotificationService
{
    public Task SendWelcomeNotificationAsync(string email);
}
