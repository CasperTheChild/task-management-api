using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Infrastructure.Context;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoListDbContext context;

    private readonly ICurrentUserService user;

    public TaskRepository(TodoListDbContext context, ICurrentUserService user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<TaskModel> CreateAsync(int todoListId, TaskCreateModel model)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = TaskMapper.ToEntityFromCreate(todoListId, model);

        await this.context.Tasks.AddAsync(entity);
        await this.context.SaveChangesAsync();

        var taskAssignmentEntity = TaskAssignmentMapper.ToTaskAssignmentEntity(entity.Id, userId);
        await this.context.TaskAssignments.AddAsync(taskAssignmentEntity);
        await this.context.SaveChangesAsync();

        return TaskMapper.ToModel(entity);
    }

    public async Task<bool> DeleteAsync(int todoListId, int id)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var canModify = await this.context.TodoListUsers.Where(t => t.TodoListId == todoListId && t.UserId == userId).FirstOrDefaultAsync();

        if (canModify == null)
        {
            return false;
        }

        var entity = await this.context.Tasks.Where(t => t.Id == id && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        this.context.Tasks.Remove(entity);
        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<PaginatedModel<TaskModel>> GetAllAsync(int todoListId, int pageNum, int pageSize)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var canView = await this.context.TodoListUsers.Where(t => t.UserId == userId && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (canView == null)
        {
            return new PaginatedModel<TaskModel>() { };
        }

        var models = await this.context.Tasks.Where(t => t.TodoListId == todoListId).ToListAsync();

        return PaginationMapper.ToPaginatedModel(models.Skip((pageNum - 1) * pageSize).Take(pageSize).Select(entity => TaskMapper.ToModel(entity)), models.Count, pageNum, pageSize);
    }

    public async Task<IEnumerable<TaskModel>> GetAllAsync(int todoListId)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var canView = await this.context.TodoListUsers.Where(t => t.UserId == userId && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (canView == null)
        {
            return new List<TaskModel>() { };
        }

        var entities = await this.context.Tasks.Where(t => t.TodoListId == todoListId).ToListAsync();

        return entities.Select(entity => TaskMapper.ToModel(entity));
    }

    public async Task<TaskModel?> GetAsync(int todoListId, int id)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");


        var canView = await this.context.TodoListUsers.Where(t => t.UserId == userId && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (canView == null)
        {
            return null;
        }

        var entity = await this.context.Tasks.Where(t => t.Id == id && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        return TaskMapper.ToModel(entity);
    }

    public async Task<TaskModel?> Patch(int todoListId, int id, JsonPatchDocument<TaskUpdateModel> patchDoc)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var canView = await this.context.TodoListUsers.Where(t => t.UserId == userId && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (canView == null)
        {
            return null;
        }

        var entity = this.context.Tasks.Where(t => t.Id == id && t.TodoListId == todoListId).FirstOrDefault();

        if (entity == null)
        {
            return null;
        }

        var model = new TaskUpdateModel();

        patchDoc.ApplyTo(model);
        TaskMapper.UpdateEntity(entity, model);

        await this.context.SaveChangesAsync();

        return TaskMapper.ToModel(entity);
    }

    public async Task<bool> UpdateAsync(int todoListId, int id, TaskCreateModel model)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var canView = await this.context.TodoListUsers.Where(t => t.UserId == userId && t.TodoListId == todoListId).FirstOrDefaultAsync();

        if (canView == null)
        {
            return false;
        }

        var entity = await this.context.Tasks.Where(t => t.Id == id && t.TodoListId == todoListId && t.AssignedUsers.Any(u => u.UserId == userId)).FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        TaskMapper.UpdateEntity(entity, model);
        await this.context.SaveChangesAsync();

        return true;
    }
}
