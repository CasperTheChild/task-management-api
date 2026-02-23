using Domain.Enums;

namespace Application.Repository.Interfaces;

public interface IAuthorizationRepository
{
    public Task<bool> IsOwnerAsync(string userId, int todoListId);

    public Task<bool> CanEditAsync(string userId, int todoListId);

    public Task<bool> CanViewAsync(string userId, int todoListId);

    public Task<bool> CanEditTasksAsync(string userId, int taskId);

    public Task<bool> CanViewTasksAsync(string userId, int taskId);

    public Task<bool> CanAssignRoleAsync(string userId, int todoListId);

    public Task<bool> AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role);
}
