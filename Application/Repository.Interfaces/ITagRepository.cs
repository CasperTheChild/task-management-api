using Application.DTOs;
using Domain.Entities;

namespace Application.Repository.Interfaces;

public interface ITagRepository
{
    public Task<IEnumerable<TagEntity>> GetAllTags();

    public Task<PaginatedModel<TagEntity>> GetTodoListTags(int todoListId, int pageNumber, int pageSize);

    public Task<TagEntity?> GetTagById(int tagId);

    public Task<TagEntity?> GetTagByName(string tagName);

    public TagEntity CreateTag(int todoListId, TagCreateModel model);

    public Task UpdateTag(int tagId, TagCreateModel model);

    public Task DeleteTag(int tagId);
}
