using Microsoft.EntityFrameworkCore;
using TodoList.Services.Database.Context;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Enums;

namespace TodoList.Services.Database.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly TodoListDbContext context;

    public AuthorizationService(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CanEditAsync(string userId, int todoListId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return false;
        }

        if (entity.Role >= TodoListRole.Editor)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CanEditTasksAsync(string userId, int taskId)
    {
        var entity = await this.context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

        if (entity == null)
        {
            return false;
        }

        var todoListId = entity.TodoListId;

        var todoListUser = this.context.TodoListUsers.Find(todoListId, userId);

        if (todoListUser == null)
        {
            return false;
        }

        if (todoListUser.Role >= TodoListRole.Editor)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CanViewAsync(string userId, int todoListId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return false;
        }

        if (entity.Role >= TodoListRole.Viewer)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CanViewTasksAsync(string userId, int taskId)
    {
        var entity = await this.context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

        if (entity == null)
        {
            return false;
        }

        var todoListId = entity.TodoListId;

        var todoListUser = this.context.TodoListUsers.Find(todoListId, userId);

        if (todoListUser == null)
        {
            return false;
        }

        if (todoListUser.Role >= TodoListRole.Viewer)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsOwnerAsync(string userId, int todoListId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return false;
        }

        if (entity.Role == TodoListRole.Owner)
        {
            return true;
        }

        return false;
    }
}
