using Application.DTOs;
using Application.Exceptions;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services;

public class TaskService
{
    private readonly ITaskRepository repository;
    private readonly IAuthorizationRepository authorizationRepository;
    private readonly ICurrentUserService currentUserService;

    public TaskService(ITaskRepository repository, IAuthorizationRepository authorizationRepository, ICurrentUserService currentUserService)
    {
        this.repository = repository;
        this.authorizationRepository = authorizationRepository;
        this.currentUserService = currentUserService;
    }

    public async Task<TaskModel> GetByIdAsync(int todoListId, int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewTasksAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var model = await this.repository.GetAsync(todoListId, id);

        if (model == null)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        return model;
    }

    public async Task<PaginatedModel<TaskModel>> GetAllAsync(int todoListId, int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewTasksAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var models = await this.repository.GetAllAsync(todoListId, pageNum, pageSize);

        return models;
    }

    public async Task<IEnumerable<TaskModel>> GetAllAsync(int todoListId)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewTasksAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var models = await this.repository.GetAllAsync(todoListId);

        return models;
    }

    public async Task<TaskModel> PatchAsync(int todoListId, int id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TaskUpdateModel> patchDoc)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.Patch(todoListId, id, patchDoc);
        if (entity == null)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        return entity;
    }

    public async Task<TaskModel> PostAsync(int todoListId, TaskCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.CreateAsync(todoListId, model);

        return entity;
    }

    public async Task DeleteAsync(int todoListId, int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.repository.DeleteAsync(todoListId, id);

        if (!success)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        return;
    }

    public async Task UpdateAsync(int todoListId, int id, TaskCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.repository.UpdateAsync(todoListId, id, model);

        if (!success)
        {
            throw new NotFoundException(nameof(TaskModel), id);
        }

        return;
    }
}
