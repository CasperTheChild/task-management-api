using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(TaskId), nameof(UserId))]
public class TaskAssignmentEntity
{
    public int TaskId { get; set; }

    public TaskEntity Task { get; set; }

    public string UserId { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
}
