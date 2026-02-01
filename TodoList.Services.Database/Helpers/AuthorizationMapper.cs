using TodoList.Services.Database.Entities;
using TodoList.WebApi.Models.Enums;

namespace TodoList.Services.Database.Helpers;

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
