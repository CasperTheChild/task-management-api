using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TodoList.Services.Database.Context;
using TodoList.Services.Database.Helpers;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Enums;
using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Services;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext context;

    private readonly ICurrentUserService user;

    public TodoListRepository(TodoListDbContext context, ICurrentUserService user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<TodoListModel> CreateAsync(TodoListCreateModel model)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = TodoListMapper.ToEntityFromCreate(model);

        await this.context.TodoLists.AddAsync(entity);
        await this.context.SaveChangesAsync();

        var todoListUserEntity = TodoListUserMapper.ToEntity(entity.Id, userId, TodoListRole.Owner);

        await this.context.TodoListUsers.AddAsync(todoListUserEntity);
        await this.context.SaveChangesAsync();

        return TodoListMapper.ToModel(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = await this.context.TodoListUsers
            .Where(tu => tu.UserId == userId && tu.TodoListId == id && tu.Role > TodoListRole.Viewer)
            .Select(tu => tu.TodoList)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        this.context.TodoLists.Remove(entity);
        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<PaginatedModel<TodoListModel>?> GetAllAsync(int pageNum, int pageSize)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var query = this.context.TodoLists
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId));

        if (query == null)
        {
            return new PaginatedModel<TodoListModel>
            {
                Items = Array.Empty<TodoListModel>(),
                TotalItems = 0,
                ItemsPerPage = pageSize,
                CurrentPage = pageNum,
            };
        }

        var totalItems = await query.CountAsync();

        var entities = await query.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        var res = entities.Select(entity => TodoListMapper.ToModel(entity));

        return PaginationMapper.ToPaginatedModel(res, totalItems, pageNum, pageSize);
    }

    public async Task<IEnumerable<TodoListModel>?> GetAllAsync()
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entities = await this.context.TodoLists
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .ToListAsync();

        return entities.Select(entity => TodoListMapper.ToModel(entity));
    }

    public async Task<PaginatedModel<TodoListPreviewModel>> GetAllPreviewAsync(int pageNum, int pageSize)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var query = this.context.TodoLists
            .Where(t => t.Members.Any(m => m.UserId == userId));

        var totalItems = await query.CountAsync();

        query = query.Skip((pageNum - 1) * pageSize).Take(pageSize).Include(t => t.Tasks);

        var entities = await query.ToListAsync();

        var models = entities.Select(t => TodoListMapper.ToPreviewModel(t));

        return PaginationMapper.ToPaginatedModel(models, models.Count(), pageNum, pageSize);
    }

    public async Task<TodoListModel?> GetAsync(int id)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = await this.context.TodoLists
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId) && t.Id == id)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        return TodoListMapper.ToModel(entity);
    }

    public async Task<TodoListModel?> PatchAsync(int id, JsonPatchDocument<TodoListUpdateModel> patchDoc)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = await this.context.TodoListUsers
            .Where(t => t.TodoListId == id && t.UserId == userId && t.Role > TodoListRole.Viewer)
            .Select(tu => tu.TodoList)
            .SingleOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        var model = new TodoListUpdateModel
        {
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
        };

        patchDoc.ApplyTo(model);
        TodoListMapper.UpdateEntity(entity, model);
        await this.context.SaveChangesAsync();

        return TodoListMapper.ToModel(entity);
    }

    public async Task<bool> UpdateAsync(int id, TodoListCreateModel model)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = await this.context.TodoListUsers
            .Where(tu => tu.TodoListId == id && tu.UserId == userId && tu.Role > TodoListRole.Viewer)
            .Select(tu => tu.TodoList)
            .SingleOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        TodoListMapper.UpdateEntity(entity, model);
        await this.context.SaveChangesAsync();

        return true;
    }
}
