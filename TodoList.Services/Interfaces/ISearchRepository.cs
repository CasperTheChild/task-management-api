using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Interfaces;

public interface ISearchRepository
{
    Task<IEnumerable<TaskModel>> SearchAsync(SearchParameterModel model);

    Task<PaginatedModel<TaskModel>> SearchPagedAsync(SearchParameterModel model, int pageNum, int pageSize);
}