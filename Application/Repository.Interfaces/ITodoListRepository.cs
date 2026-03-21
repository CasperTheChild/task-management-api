using Application.DTOs;
using Domain.Entities;

namespace Application.Repository.Interfaces;

public interface ITodoListRepository
{
    Task<TodoListEntity?> GetAsync(int id);

    void Create(TodoListEntity entity);

    Task UpdateAsync(int id, TodoListEntity entity);

    Task DeleteAsync(int id);

    Task<IEnumerable<TodoListEntity>> GetAllAsync(string userId);

    Task<PaginatedModel<TodoListEntity>> GetAllAsync(string userId, int pageNum, int pageSize);

    Task<PaginatedModel<TodoListPreviewModel>> GetAllPreviewAsync(string userId, int pageNum, int pageSize, int amount);
}
