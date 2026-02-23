using Domain.Enums;

namespace Application.DTOs;

public class SearchSortModel
{
    public TaskSortOption SortOption { get; set; } = TaskSortOption.CreatedAtAsc;
}
