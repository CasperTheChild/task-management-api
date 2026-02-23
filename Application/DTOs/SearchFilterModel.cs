using Domain.Enums;

namespace Application.DTOs;

public class SearchFilterModel
{
    public int? TodoListId { get; set; }
    public int? TagId { get; set; }

    public TaskStatusFilter? TaskStatus { get; set; }

    public DateTime? DueFrom { get; set; }
    public DateTime? DueTo { get; set; }
}
