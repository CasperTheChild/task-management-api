using Domain.Enums;

namespace Application.Repository.Interfaces;

public interface ITodoListUserRepository
{
    public Task<bool> AssignPermissionAsync(int todoListId, string userId, TodoListRole role);

    public Task<bool> RemovePermissionAsync(int todoListId, string userId);

    public Task<TodoListRole?> HasPermissionAsync(int todoListId, string userId);

    public Task<bool> UpdatePermissionAsync(int todoListId, string userId, TodoListRole role);
}
