using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services;

public class TodoListService
{
    private readonly ITodoListRepository repository;
    private readonly ICurrentUserService currentUserService;
    private readonly IAuthorizationRepository authorizationRepository;

    public TodoListService(ITodoListRepository repository, ICurrentUserService userService, IAuthorizationRepository authorizationRepository)
    {
        this.repository = repository;
        this.currentUserService = userService;
        this.authorizationRepository = authorizationRepository;
    }

    public async Task<IEnumerable<TodoListModel>> GetAllAsync()
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var models = await this.repository.GetAllAsync(userId);

        return models;
    }

    public async Task<ActionResult<PaginatedModel<TodoListModel>>> GetAllAsync(int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var models = await this.repository.GetAllAsync(userId, pageNum, pageSize);

        return models;
    }

    public async Task<TodoListModel> GetByIdAsync(int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var model = await this.repository.GetAsync(userId, id);

        if (model == null)
        {
            throw new UnauthorizedAccessException();
        }

        return model;
    }

    public async Task<PaginatedModel<TodoListPreviewModel>> GetAllPreviewAsync(int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var models = await this.repository.GetAllPreviewAsync(userId, pageNum, pageSize);

        return models;
    }

    public async Task<TodoListModel> CreateAsync(TodoListCreateModel model)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var createdModel = await this.repository.CreateAsync(userId, model);
        return createdModel;
    }

    public async Task<TodoListModel?> PatchAsync(int id, JsonPatchDocument<TodoListUpdateModel> patchDoc)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var updatedModel = await this.repository.PatchAsync(userId, id, patchDoc);

        if (updatedModel == null)
        {
            throw new UnauthorizedAccessException();
        }

        return updatedModel;
    }

    public async Task<bool> UpdateAsync(int id, TodoListCreateModel model)
    {
        var userId = this.currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.repository.UpdateAsync(userId, id, model);
        if (!success)
        {
            throw new UnauthorizedAccessException();
        }

        return success;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.repository.DeleteAsync(userId, id);

        if (!success)
        {
            throw new UnauthorizedAccessException();
        }

        return success;
    }
}
