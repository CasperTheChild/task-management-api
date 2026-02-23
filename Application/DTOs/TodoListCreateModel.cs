using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TodoListCreateModel
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.Today;
}
