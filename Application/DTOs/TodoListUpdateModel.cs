using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TodoListUpdateModel
{
    public string? Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; } = DateTime.Today;
}
