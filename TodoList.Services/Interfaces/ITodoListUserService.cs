using TodoList.WebApi.Models.Enums;
using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Interfaces;

public interface ITodoListUserService
{
    public Task<bool> AssignPermissionAsync(int todoListId, string userId, TodoListRole role);

    public Task<bool> RemovePermissionAsync(int todoListId, string userId);

    public Task<TodoListRole?> HasPermissionAsync(int todoListId, string userId);

    public Task<bool> UpdatePermissionAsync(int todoListId, string userId, TodoListRole role);
}
