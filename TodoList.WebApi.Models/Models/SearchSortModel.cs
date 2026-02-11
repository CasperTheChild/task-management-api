using TodoList.Services.Enums;

namespace TodoList.WebApi.Models.Models;

public class SearchSortModel
{
    public TaskSortOption SortOption { get; set; } = TaskSortOption.CreatedAtAsc;
}
