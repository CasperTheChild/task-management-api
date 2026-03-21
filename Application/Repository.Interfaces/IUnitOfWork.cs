namespace Application.Repository.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
