using Application.Services.Interfaces;
using Hangfire;

namespace Application.Services;

public class AuthenticationService
{
    private readonly IAuthService service;
    private readonly INotificationService notificationService;

    public AuthenticationService(IAuthService service, INotificationService notificationService)
    {
        this.service = service;
        this.notificationService = notificationService;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var rhuh = await this.service.LoginAsync(username, password);

        BackgroundJob.Enqueue(() => this.notificationService.SendLoginNotificationAsync(username));

        return rhuh;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        var rhuh = await this.service.RegisterAsync(email, password);

        BackgroundJob.Enqueue(() => this.notificationService.SendNotificationAsync(email, "Welcome!", "Thank you for registering.", false));

        return rhuh;
    }
}
