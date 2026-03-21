using Domain.Enums;

namespace Application.Repository.Interfaces;

public interface IAuthorizationRepository
{
    Task AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role);

    Task<TodoListRole?> GetRoleAsync(int todoListId, string userId);
}
