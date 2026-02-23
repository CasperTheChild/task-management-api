namespace Application.DTOs;

public class TaskUpdateModel
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsCompleted { get; set; }
}
