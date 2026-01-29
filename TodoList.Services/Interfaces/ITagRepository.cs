using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Interfaces;

public interface ITagRepository
{
    public Task<IEnumerable<TagModel>> GetAllTags();

    public Task<PaginatedModel<TagModel>> GetTodoListTags(int todoListId, int pageNumber, int pageSize);

    public Task<TagModel?> GetTagById(int tagId);

    public Task<TagModel?> GetTagByName(string tagName);

    public Task<PaginatedModel<TaskModel>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize);

    public Task<PaginatedModel<TagModel>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize);

    public Task<TagModel> CreateTag(int todoListId, TagCreateModel model);

    public Task<TagModel> UpdateTag(int tagId, TagCreateModel model);

    public Task<bool> DeleteTag(int tagId);

    public Task<bool> AssignTag(int taskId, int tagId);

    public Task<bool> RemoveTag(int taskId, int tagId);
}
