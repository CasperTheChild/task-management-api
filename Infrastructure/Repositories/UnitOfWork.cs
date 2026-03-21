using Application.Repository.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TodoListDbContext context;

    public UnitOfWork(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        var affected = await this.context.SaveChangesAsync();

        return affected;
    }
}
