using Application.Repository.Interfaces;
using Domain.Enums;
using Infrastructure.Context;
using Application.Helpers;

namespace Infrastructure.Repositories;

public class TodoListUserRepository : ITodoListUserRepository
{
    private readonly TodoListDbContext context;

    public TodoListUserRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task AssignRoleAsync(int todoListId, string userId, TodoListRole role)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity != null)
        {
            return;
        }

        entity = TodoListUserMapper.ToEntity(todoListId, userId, role);

        await this.context.TodoListUsers.AddAsync(entity);
    }

    public async Task<TodoListRole?> HasRoleAsync(int todoListId, string userId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return null;
        }

        return entity.Role;
    }

    public async Task RemoveRoleAsync(int todoListId, string userId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity != null)
        {
            this.context.TodoListUsers.Remove(entity);
        }

    }

    public async Task UpdateRoleAsync(int todoListId, string userId, TodoListRole role)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity != null)
        {
            this.context.TodoListUsers.Update(entity);
        }
    }
}
