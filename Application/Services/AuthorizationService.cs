using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class AuthorizationService
{
    private readonly IAuthorizationRepository repository;
    private readonly ICurrentUserService userService;
    private readonly IUnitOfWork unitOfWork;

    public AuthorizationService(IAuthorizationRepository repository, ICurrentUserService service, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.userService = service;
        this.unitOfWork = unitOfWork;
    }

    public async Task AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role)
    {
        var userId = this.userService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException("UserId is not found!");
        }

        var canAssign = await this.CanAssignRoleAsync(userId, todoListId);

        if (!canAssign)
        {
            throw new UnauthorizedAccessException("The current userId can not assign roles");
        }

        await this.repository.AssignRoleAsync(todoListId, targetUserId, role);
    }

    public async Task<bool> CanAssignRoleAsync(string userId, int todoListId)
    {
        var role = await this.repository.GetRoleAsync(todoListId, userId);

        if (role == null)
        {
            return false;
        }

        if (role.Value >= TodoListRole.Owner)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CanEditAsync(string userId, int todoListId)
    {
        var role = await this.repository.GetRoleAsync(todoListId, userId);

        if (role == null)
        {
            return false;
        }

        if (role.Value >= TodoListRole.Editor)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CanViewAsync(string userId, int todoListId)
    {
        var role = await this.repository.GetRoleAsync(todoListId, userId);

        if (role == null)
        {
            return false;
        }

        if (role.Value >= TodoListRole.Viewer)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsOwnerAsync(string userId, int todoListId)
    {
        var role = await this.repository.GetRoleAsync(todoListId, userId);

        if (role == null)
        {
            return false;
        }

        if (role.Value >= TodoListRole.Owner)
        {
            return true;
        }

        return false;
    }
}