using Application.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Repository.Interfaces;

public interface ITodoListRepository
{
    Task<TodoListModel?> GetAsync(string userId, int id);

    Task<TodoListModel> CreateAsync(string userId, TodoListCreateModel model);

    Task<bool> UpdateAsync(string userId, int id, TodoListCreateModel model);

    Task<TodoListModel?> PatchAsync(string userId, int id, JsonPatchDocument<TodoListUpdateModel> patchDoc);

    Task<bool> DeleteAsync(string userId, int id);

    Task<IEnumerable<TodoListModel>> GetAllAsync(string userId);

    Task<PaginatedModel<TodoListModel>> GetAllAsync(string userId, int pageNum, int pageSize);

    Task<PaginatedModel<TodoListPreviewModel>> GetAllPreviewAsync(string userId, int pageNum, int pageSize);
}
