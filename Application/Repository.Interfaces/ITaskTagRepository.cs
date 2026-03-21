using Application.DTOs;
using Domain.Entities;

namespace Application.Repository.Interfaces;

public interface ITaskTagRepository
{
    public void AssignTag(int taskId, int tagId);

    public Task RemoveTag(int taskId, int tagId);

    public Task<PaginatedModel<TaskEntity>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize);

    public Task<PaginatedModel<TagEntity>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize);
}
