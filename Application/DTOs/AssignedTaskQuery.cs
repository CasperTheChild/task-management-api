namespace Domain.Enums;

public class AssignedTaskQuery
{
    public TaskStatusFilter StatusFilter { get; set; } = TaskStatusFilter.All;

    public TaskSortOption SortOption { get; set; } = TaskSortOption.CreatedAtDesc;

    public int PageNum { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
