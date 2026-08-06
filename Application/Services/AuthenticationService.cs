using Application.Services.Interfaces;

namespace Application.Services;

public class AuthenticationService
{
    private readonly IAuthService service;
    private readonly IBackgroundJobService backgroundJobService;
    private readonly INotificationService notificationService;

    public AuthenticationService(IAuthService service, IBackgroundJobService backgroundJobService, INotificationService notificationService)
    {
        this.service = service;
        this.backgroundJobService = backgroundJobService;
        this.notificationService = notificationService;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var rhuh = await this.service.LoginAsync(username, password);
        //this.backgroundJobService.Enqueue<INotificationService>(x => x.SendWelcomeNotificationAsync(username));
        return rhuh;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        var rhuh = await this.service.RegisterAsync(email, password);
        //this.backgroundJobService.Enqueue<INotificationService>(x => x.SendWelcomeNotificationAsync(email));
        return rhuh;
    }
}
