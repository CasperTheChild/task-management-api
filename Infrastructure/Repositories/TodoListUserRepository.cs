using Application.Repository.Interfaces;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Repositories;

public class TodoListUserRepository : ITodoListUserRepository
{
    private readonly TodoListDbContext context;

    public TodoListUserRepository(TodoListDbContext context)
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
