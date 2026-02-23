using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services;

public class TaskAssignmentService
{
    private readonly ITaskAssignmentRepository repository;
    private readonly IAuthorizationRepository authorizationService;
    private readonly ICurrentUserService currentUserService;

    public TaskAssignmentService(ITaskAssignmentRepository repository, IAuthorizationRepository authorizationRepository, ICurrentUserService currentUserService)
    {
        this.repository = repository;
        this.authorizationService = authorizationRepository;
        this.currentUserService = currentUserService;
    }

    public async Task<PaginatedModel<TaskModel>> GetPagedAsync(AssignedTaskQuery query)
    {
        var models = await this.repository.GetAssignedTasks(query);
        return models;
    }

    public async Task DeleteAsync(string userId, int taskId)
    {
        var currentId = this.currentUserService.UserId;

        if (currentId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.RemoveTaskAssignmentAsync(userId, taskId);

        return;
    }

    public async Task PostAsync(string userId, int taskId)
    {
        var currentUserId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(currentUserId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.AssignTaskToUserAsync(userId, taskId);

        return;
    }
}
