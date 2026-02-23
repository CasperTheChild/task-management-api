using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TaskModel
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime? EndDate { get; set; }

    public bool IsCompleted { get; set; }

    public int TodoListId { get; set; }
}
