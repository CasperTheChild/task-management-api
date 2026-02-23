using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

public class AuthorizationService
{
    private readonly IAuthorizationRepository repository;
    private readonly ICurrentUserService userService;

    public AuthorizationService(IAuthorizationRepository repository, ICurrentUserService service)
    {
        this.repository = repository;
        this.userService = service;
    }

    public async Task<bool> AssignRole(int todoListId, string targetUserId, Domain.Enums.TodoListRole role)
    {
        var userId = this.userService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException("UserId is not found!");
        }

        var canAssign = await this.repository.CanAssignRoleAsync(userId, todoListId);

        if (!canAssign)
        {
            throw new UnauthorizedAccessException("The current userId can not assign roles");
        }

        var success = await this.repository.AssignRoleAsync(todoListId, targetUserId, role);

        if (!success)
        {
            throw new Exception("Couldn't assign");
        }

        return true;
    }
}