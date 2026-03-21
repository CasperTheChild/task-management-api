using Application.Repository.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class TodoListUserService
{
    private readonly ITodoListUserRepository repository;
    private readonly IUnitOfWork unitOfWork;

    public TodoListUserService(ITodoListUserRepository repository, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }

    public async Task AssignRoleAsync(int todoListId, string userId, TodoListRole role)
    {
        var entity = this.repository.AssignRoleAsync(todoListId, userId, role);

        await this.unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveRoleAsync(int todoListId, string userId)
    {
        var entity = this.repository.RemoveRoleAsync(todoListId, userId);
        await this.unitOfWork.SaveChangesAsync();
    }

    public Task<TodoListRole?> HasRoleAsync(int todoListId, string userId)
    {
        return this.repository.HasRoleAsync(todoListId, userId);
    }

    public async Task UpdateRoleAsync(int todoListId, string userId, TodoListRole role)
    {
        var entity = this.repository.UpdateRoleAsync(todoListId, userId, role);
        await this.unitOfWork.SaveChangesAsync();
    }
}
