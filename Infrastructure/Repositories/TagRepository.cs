using Application.DTOs;
using Application.Repository.Interfaces;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly TodoListDbContext context;

    public TagRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public TagEntity CreateTag(int todoListId, TagCreateModel model)
    {
        var entity = TagMapper.ToEntity(todoListId, model);

        this.context.Tags.Add(entity);

        return entity;
    }

    public async Task DeleteTag(int tagId)
    {
        var entity = await this.context.Tags.FindAsync(tagId);

        if (entity != null)
        {
            this.context.Tags.Remove(entity);
        }
    }

    public async Task<IEnumerable<TagEntity>> GetAllTags()
    {
        return await this.context.Tags
            .ToListAsync();
    }

    public async Task<TagEntity?> GetTagById(int tagId)
    {
        var entity = await this.context.Tags.FindAsync(tagId);

        if (entity != null)
        {
            return entity;
        }

        return null;
    }

    public async Task<TagEntity?> GetTagByName(string tagName)
    {
        var entity = await this.context.Tags
            .FirstOrDefaultAsync(t => t.NormalizedTagName == tagName.ToLower());

        if (entity != null)
        {
            return entity;
        }

        return null;
    }

    public Task<PaginatedModel<TagEntity>> GetTodoListTags(int todoListId, int pageNumber, int pageSize)
    {
        var query = this.context.Tags
            .Where(t => t.TodoListId == todoListId);

        var totalItems = query.Count();

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var paginatedModel = PaginationMapper.ToPaginatedEntity(items, totalItems, pageNumber, pageSize);

        return Task.FromResult(paginatedModel);
    }

    public async Task UpdateTag(int tagId, TagCreateModel model)
    {
        var entity = await this.context.Tags
            .FindAsync(tagId);

        if (entity == null)
        {
            return;
        }

        entity.TagName = model.Name;
    }
}
