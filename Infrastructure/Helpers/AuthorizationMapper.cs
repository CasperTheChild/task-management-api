using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Helpers;

public static class AuthorizationMapper
{
    public static TodoListUserEntity ToEntity(int todoListId, string userId, TodoListRole role)
    {
        return new TodoListUserEntity()
        {
            TodoListId = todoListId,
            UserId = userId,
            Role = role,
        };
    }
}
