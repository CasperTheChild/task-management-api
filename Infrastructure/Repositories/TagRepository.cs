using Application.DTOs;
using Application.Repository.Interfaces;
using Infrastructure.Context;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly TodoListDbContext context;

    public TagRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AssignTag(int taskId, int tagId)
    {
        var taskEntity = await this.context.Tasks.FindAsync(taskId);

        if (taskEntity == null)
        {
            throw new InvalidDataException("NO such task exists.");
        }

        var tagEntity = await this.context.Tags.Where(t => t.Id == tagId && t.TodoListId == taskEntity.TodoListId).FirstOrDefaultAsync();

        if (tagEntity == null)
        {
            throw new InvalidOperationException($"Tag with ID {tagId} not found in the same TodoList as the task.");
        }

        var existingAssignment = await this.context.TaskTags.FindAsync(taskId, tagId);

        if (existingAssignment != null)
        {
            return true; // Tag is already assigned to the task
        }

        var taskTagEntity = TagMapper.ToTaskTagEntity(taskId, tagId);

        this.context.TaskTags.Add(taskTagEntity);

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<TagModel> CreateTag(int todoListId, TagCreateModel model)
    {
        var entity = await this.context.Tags.FirstOrDefaultAsync(t => t.NormalizedTagName == model.Name.ToLower() && t.TodoListId == todoListId);

        if (entity != null)
        {
            return TagMapper.ToModel(entity);
        }

        entity = TagMapper.ToEntity(todoListId, model);

        this.context.Tags.Add(entity);

        await this.context.SaveChangesAsync();

        return TagMapper.ToModel(entity);
    }

    public async Task<bool> DeleteTag(int tagId)
    {
        var entity = await this.context.Tags.FindAsync(tagId);

        if (entity == null)
        {
            return false;
        }

        this.context.Tags.Remove(entity);
        await this.context.SaveChangesAsync();

        return true;
    }

    public Task<IEnumerable<TagModel>> GetAllTags()
    {
        IEnumerable<TagModel> tags = this.context.Tags
            .Select(t => TagMapper.ToModel(t));

        return Task.FromResult(tags);
    }

    public Task<PaginatedModel<TagModel>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize)
    {
        var query = this.context.TaskTags
            .Where(tt => tt.TaskId == taskId)
            .Select(tt => tt.Tag)
            .Select(t => TagMapper.ToModel(t));

        var totalItems = query.Count();

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var paginatedModel = PaginationMapper.ToPaginatedModel(items, totalItems, pageNumber, pageSize);

        return Task.FromResult(paginatedModel);
    }

    public Task<PaginatedModel<TaskModel>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize)
    {
        var query = this.context.TaskTags
            .Where(tt => tt.TagId == tagId)
            .Select(tt => tt.Task)
            .Select(t => TaskMapper.ToModel(t));
        var totalItems = query.Count();
        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var paginatedModel = PaginationMapper.ToPaginatedModel(items, totalItems, pageNumber, pageSize);
        return Task.FromResult(paginatedModel);
    }

    public async Task<TagModel?> GetTagById(int tagId)
    {
        var entity = await this.context.Tags.FindAsync(tagId);

        if (entity == null)
        {
            return null;
        }

        return TagMapper.ToModel(entity);
    }

    public async Task<TagModel?> GetTagByName(string tagName)
    {
        var entity = await this.context.Tags
            .FirstOrDefaultAsync(t => t.NormalizedTagName == tagName.ToLower());

        if (entity == null)
        {
            return null;
        }

        return TagMapper.ToModel(entity);
    }

    public Task<PaginatedModel<TagModel>> GetTodoListTags(int todoListId, int pageNumber, int pageSize)
    {
        var query = this.context.Tags
            .Where(t => t.TodoListId == todoListId)
            .Select(t => TagMapper.ToModel(t));

        var totalItems = query.Count();

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var paginatedModel = PaginationMapper.ToPaginatedModel(items, totalItems, pageNumber, pageSize);

        return Task.FromResult(paginatedModel);
    }

    public async Task<bool> RemoveTag(int taskId, int tagId)
    {
        var entity = await this.context.TaskTags.FindAsync(taskId, tagId);

        if (entity == null)
        {
            return false;
        }

        this.context.TaskTags.Remove(entity);

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<TagModel> UpdateTag(int tagId, TagCreateModel model)
    {
        var entity = await this.context.Tags
            .FindAsync(tagId);

        if (entity == null)
        {
            throw new InvalidOperationException($"Tag with ID {tagId} not found.");
        }

        entity.TagName = model.Name;

        await this.context.SaveChangesAsync();

        return TagMapper.ToModel(entity);
    }
}
