using Application.DTOs;
using Application.Exceptions;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;
using Application.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Domain.Entities;

namespace Application.Services;

public class TodoListService
{
    private readonly ITodoListRepository repository;
    private readonly ICurrentUserService currentUserService;
    private readonly AuthorizationService authorizationService;
    private readonly ITodoListUserRepository todoListUserRepository;
    private readonly IUnitOfWork unitOfWork;

    public TodoListService(ITodoListRepository repository, ICurrentUserService userService, AuthorizationService authorizationService, ITodoListUserRepository todoListUserRepository, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.currentUserService = userService;
        this.authorizationService = authorizationService;
        this.todoListUserRepository = todoListUserRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TodoListModel>> GetAllAsync()
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var entities = await this.repository.GetAllAsync(userId);

        return entities.Select(t => TodoListMapper.ToModel(t));
    }

    public async Task<PaginatedModel<TodoListModel>> GetAllAsync(int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var entities = await this.repository.GetAllAsync(userId, pageNum, pageSize);

        var models = new PaginatedModel<TodoListModel>
        {
            Items = entities.Items.Select(t => TodoListMapper.ToModel(t)),
            TotalItems = entities.TotalItems,
            ItemsPerPage = entities.ItemsPerPage,
            CurrentPage = entities.CurrentPage,
        };

        return models;
    }

    public async Task<TodoListModel> GetAsync(int id)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetAsync(id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), id);
        }

        return TodoListMapper.ToModel(entity);
    }

    public async Task<PaginatedModel<TodoListPreviewModel>> GetAllPreviewAsync(int pageNum, int pageSize, int amount)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var models = await this.repository.GetAllPreviewAsync(userId, pageNum, pageSize, amount);

        return models;
    }

    public async Task<TodoListModel> CreateAsync(TodoListCreateModel model)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var entity = TodoListMapper.ToEntityFromCreate(model);

        this.repository.Create(entity);

        var affected = await this.unitOfWork.SaveChangesAsync();

        await this.todoListUserRepository.AssignRoleAsync(entity.Id, userId, TodoListRole.Owner);

        await this.unitOfWork.SaveChangesAsync();

        return TodoListMapper.ToModel(entity);
    }

    public async Task PatchAsync(int id, JsonPatchDocument<TodoListUpdateModel> patchDoc)
    {
        var userId = this.currentUserService.UserId ?? throw new UnauthorizedAccessException();

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetAsync(id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), id);
        }

        var updatedModel = new TodoListUpdateModel
        {
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
        };

        patchDoc.ApplyTo(updatedModel);

        TodoListMapper.ToEntityFromUpdate(entity, updatedModel);

        await this.repository.UpdateAsync(id, entity);

        await this.unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, TodoListCreateModel model)
    {
        var userId = this.currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        TodoListEntity entity = TodoListMapper.ToEntityFromCreate(model);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), id);
        }

        await this.repository.UpdateAsync(id, entity);

        await this.unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.DeleteAsync(id);

        await this.unitOfWork.SaveChangesAsync();
    }
}
