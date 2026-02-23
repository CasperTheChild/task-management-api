namespace Domain.Entities;

public class TaskTagEntity
{
    public int TaskId { get; set; }

    public TaskEntity Task { get; set; }

    public int TagId { get; set; }

    public TagEntity Tag { get; set; }
}
