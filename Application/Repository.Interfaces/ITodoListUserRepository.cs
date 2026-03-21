using Domain.Enums;

namespace Application.Repository.Interfaces;

public interface ITodoListUserRepository
{
    public Task AssignRoleAsync(int todoListId, string userId, TodoListRole role);

    public Task RemoveRoleAsync(int todoListId, string userId);

    public Task<TodoListRole?> HasRoleAsync(int todoListId, string userId);

    public Task UpdateRoleAsync(int todoListId, string userId, TodoListRole role);
}
