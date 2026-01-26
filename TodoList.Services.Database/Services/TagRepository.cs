using Microsoft.EntityFrameworkCore;
using TodoList.Services.Database.Context;
using TodoList.Services.Database.Helpers;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Database.Services;

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
            return false;
        }

        var tagEntity = await this.context.Tags.FindAsync(tagId);

        if (tagEntity == null)
        {
            return false;
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

    public async Task<TagModel> CreateTag(TagCreateModel model)
    {
        var entity = await this.context.Tags.FirstOrDefaultAsync(t => t.NormalizedTagName == model.Name.ToLower());

        if (entity != null)
        {
            return TagMapper.ToModel(entity);
        }

        entity = TagMapper.ToEntity(model);

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

    public Task<PaginatedModel<TagModel>> GetUserTags(string userId, int pageNumber, int pageSize)
    {
        throw new NotImplementedException("");

        var query = this.context.Tags
            .Include(t => t.TaskTags)
            .ThenInclude(t => t.Task)
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
