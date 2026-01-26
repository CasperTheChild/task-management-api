using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TodoList.Services.Database.Context;
using TodoList.Services.Database.Entities;
using TodoList.Services.Database.Helpers;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Enums;

namespace TodoList.Services.Database.Services;

public class TodoListUserService : ITodoListUserService
{
    private readonly TodoListDbContext context;

    public TodoListUserService(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AssignPermissionAsync(int todoListId, string userId, TodoListRole role)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity != null)
        {
            return false;
        }

        entity = TodoListUserMapper.ToEntity(todoListId, userId, role);

        await this.context.TodoListUsers.AddAsync(entity);
        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<TodoListRole?> HasPermissionAsync(int todoListId, string userId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return null;
        }

        return entity.Role;
    }

    public async Task<bool> RemovePermissionAsync(int todoListId, string userId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return false;
        }

        this.context.TodoListUsers.Remove(entity);
        await this.context.SaveChangesAsync();

        return true;
    }

    public Task<bool> UpdatePermissionAsync(int todoListId, string userId, TodoListRole role)
    {
        var entity = this.context.TodoListUsers.Find(todoListId, userId);

        if (entity == null)
        {
            return Task.FromResult(false);
        }

        entity.Role = role;

        this.context.TodoListUsers.Update(entity);
        this.context.SaveChangesAsync();

        return Task.FromResult(true);
    }
}
