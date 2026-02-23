namespace Application.Repository.Interfaces;

public interface IAuthRepository
{
    Task<bool> RegisterAsync(string email, string password);

    Task<string?> LoginAsync(string login, string password);
}
