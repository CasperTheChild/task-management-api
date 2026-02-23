using Application.Repository.Interfaces;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly TodoListDbContext context;

    public AuthorizationRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role)
    {
        var entity = await this.context.TodoListUsers.Where(t => t.TodoListId == todoListId && t.UserId == targetUserId).FirstOrDefaultAsync();

        if (entity == null)
        {
            entity = AuthorizationMapper.ToEntity(todoListId, targetUserId, role);

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            return true;
        }

        if (entity.Role == role)
        {
            return true;
        }

        entity.Role = role;

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CanAssignRoleAsync(string userId, int todoListId)
    {
        var entity = await this.context.TodoListUsers.Where(t => t.UserId == userId && t.TodoListId == todoListId && t.Role >= TodoListRole.Owner).FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        return true;
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
