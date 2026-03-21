using Application.DTOs;
using Application.Repository.Interfaces;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TodoListDbContext context;

    public UserRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var entity = await this.context.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

        if (entity is null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ExistsByIdAsync(string userId)
    {
        var entity = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (entity is null)
        {
            return false;
        }

        return true;
    }

    public async Task<PaginatedModel<UserSummaryModel>> GetUsersAsync(int pageNum, int pageSize)
    {
        var query = this.context.Users;

        var totalItems = await query.CountAsync();

        var entities = await query.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        var res = entities.Select(entity => UserMapper.ToModel(entity));

        return PaginationMapper.ToPaginatedModel(res, totalItems, pageNum, pageSize);
    }
}
