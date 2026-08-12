using Application.Repository.Interfaces;
using Domain.Enums;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly TodoListDbContext context;

    public AuthorizationRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role)
    {
        var entity = await this.context.TodoListUsers.Where(t => t.TodoListId == todoListId && t.UserId == targetUserId).FirstOrDefaultAsync();

        if (entity == null)
        {
            entity = AuthorizationMapper.ToEntity(todoListId, targetUserId, role);

            await this.context.AddAsync(entity);
        }

        entity.Role = role;
    }

    public async Task<TodoListRole?> GetRoleAsync(int todoListId, string userId)
    {
        var entity = await this.context.TodoListUsers.FindAsync(todoListId, userId);

        if (entity == null)
        {
            return null;
        }

        return entity.Role;
    }
}
