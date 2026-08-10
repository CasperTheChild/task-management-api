namespace Application.Services.Interfaces;

public interface INotificationService
{
    public Task SendNotificationAsync(string to, string subject, string body, bool isHttp);
}
