using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Repository.Interfaces;

public interface ITaskRepository
{
    public Task<PaginatedModel<TaskEntity>> GetAllByUserIdAsync(string userId, int pageNum, int pageSize);

    public Task<TaskEntity?> GetAsync(int id);

    public Task<PaginatedModel<TaskEntity>> GetTasksDueSoon(string userid, int pageNum, int pageSize);

    public void CreateAsync(int todoListId, TaskEntity entity);

    public Task UpdateAsync(int id, TaskEntity entity);

    public Task DeleteAsync(int id);

    public Task<IEnumerable<TaskEntity>> GetAllAsync(int todoListId);

    public Task<PaginatedModel<TaskEntity>> GetAllAsync(int todoListId, int pageNum, int pageSize);
}
