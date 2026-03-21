using Application.Services.Interfaces;

namespace Application.Services;

public class AuthenticationService
{
    private readonly IAuthService service;

    public AuthenticationService(IAuthService service)
    {
        this.service = service;
    }

    public async Task<string?> Login(string username, string password)
    {
        return await this.service.LoginAsync(username, password);
    }

    public async Task<bool> Register(string email, string password)
    {
        return await this.service.RegisterAsync(email, password);
    }
}
