namespace Application.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(string email, string password);

    Task<string?> LoginAsync(string login, string password);
}
