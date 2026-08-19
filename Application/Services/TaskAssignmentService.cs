using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class TaskAssignmentService
{
    private readonly ITaskAssignmentRepository repository;
    private readonly AuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;
    private readonly IUnitOfWork unitOfWork;

    public TaskAssignmentService(ITaskAssignmentRepository repository, AuthorizationService authorizationRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.authorizationService = authorizationRepository;
        this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
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

        var permission = await this.authorizationService.CanEditAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.RemoveTaskAssignmentAsync(userId, taskId);

        await this.unitOfWork.SaveChangesAsync();

        return;
    }

    public async Task PostAsync(string userId, int taskId)
    {
        var currentUserId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(currentUserId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        this.repository.AssignTaskToUserAsync(userId, taskId);

        await this.unitOfWork.SaveChangesAsync();

        return;
    }
}
