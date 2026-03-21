using Domain.Entities;
using Domain.Enums;

namespace Application.Helpers;

public static class TodoListUserMapper
{
    public static TodoListUserEntity ToEntity(int todoListId, string userId, TodoListRole role)
    {
        return new TodoListUserEntity
        {
            TodoListId = todoListId,
            UserId = userId,
            Role = role,
        };
    }
}
