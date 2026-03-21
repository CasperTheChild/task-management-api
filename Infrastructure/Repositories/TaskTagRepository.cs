using Application.DTOs;
using Application.Helpers;
using Application.Repository.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskTagRepository : ITaskTagRepository
{
    private readonly TodoListDbContext context;

    public TaskTagRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<PaginatedModel<TagEntity>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize)
    {
        var query = this.context.TaskTags
            .Where(tt => tt.TaskId == taskId)
            .Select(tt => tt.Tag);

        var totalItems = query.Count();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PaginationMapper.ToPaginatedEntity(items, totalItems, pageNumber, pageSize);
    }

    public async Task<PaginatedModel<TaskEntity>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize)
    {
        var query = this.context.TaskTags
            .Where(tt => tt.TagId == tagId)
            .Select(tt => tt.Task);
        var totalItems = query.Count();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PaginationMapper.ToPaginatedEntity(items, totalItems, pageNumber, pageSize);
    }


    public void AssignTag(int taskId, int tagId)
    {
        var taskTagEntity = TagMapper.ToTaskTagEntity(taskId, tagId);

        this.context.TaskTags.Add(taskTagEntity);
    }

    public async Task RemoveTag(int taskId, int tagId)
    {
        var entity = await this.context.TaskTags.FindAsync(taskId, tagId);

        if (entity != null)
        {
            this.context.TaskTags.Remove(entity);
        }
    }
}
