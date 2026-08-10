using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

namespace Application.Services;

public class TaskService
{
    private readonly ITaskRepository repository;
    private readonly AuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;
    private readonly IUnitOfWork unitOfWork;
    private readonly TaskAssignmentService taskAssignmentService;

    public TaskService(ITaskRepository repository, AuthorizationService authorizationService, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, TaskAssignmentService taskAssignmentService)
    {
        this.repository = repository;
        this.authorizationService = authorizationService;
        this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
        this.taskAssignmentService = taskAssignmentService;
    }

    public async Task<TaskModel> GetAsync(int todoListId, int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetAsync(id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        return TaskMapper.ToModel(entity);
    }

    public async Task<PaginatedModel<TaskModel>> GetAllByUserIdAsync(int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var entities = await this.repository.GetAllByUserIdAsync(userId, pageNum, pageSize);

        return PaginationMapper.ToPaginatedModel(entities.Items.Select(t => TaskMapper.ToModel(t)), entities.TotalItems, pageNum, pageSize);
    }

    public async Task<PaginatedModel<TaskModel>> GetAllAsync(int todoListId, int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entities = await this.repository.GetAllAsync(todoListId, pageNum, pageSize);

        return PaginationMapper.ToPaginatedModel(entities.Items.Select(t => TaskMapper.ToModel(t)), entities.TotalItems, pageNum, pageSize);
    }

    public async Task<IEnumerable<TaskModel>> GetAllAsync(int todoListId)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entities = await this.repository.GetAllAsync(todoListId);

        return entities.Select(TaskMapper.ToModel);
    }

    public async Task PatchAsync(int todoListId, int id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TaskUpdateModel> patchDoc)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetAsync(id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        var updateModel = new TaskUpdateModel
        {
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            IsCompleted = entity.IsCompleted,
        };

        patchDoc.ApplyTo(updateModel);

        TaskMapper.UpdateEntity(entity, updateModel);

        await this.repository.UpdateAsync(id, entity);

        await this.unitOfWork.SaveChangesAsync();
    }

    public async Task<TaskModel> CreateAsync(int todoListId, TaskCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = TaskMapper.ToEntityFromCreate(todoListId, model);

        this.repository.CreateAsync(todoListId, entity);

        await this.taskAssignmentService.PostAsync(userId, entity.Id);

        await this.unitOfWork.SaveChangesAsync();

        return TaskMapper.ToModel(entity);
    }

    public async Task DeleteAsync(int todoListId, int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.DeleteAsync(id);

        await this.unitOfWork.SaveChangesAsync();

        return;
    }

    public async Task UpdateAsync(int todoListId, int id, TaskCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetAsync(id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        TaskMapper.UpdateEntity(entity, model);

        await this.repository.UpdateAsync(id, entity);

        await this.unitOfWork.SaveChangesAsync();
    }
}
