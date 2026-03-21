using Domain.Entities;

namespace Application.Helpers;

public static class TaskAssignmentMapper
{
    public static TaskAssignmentEntity ToTaskAssignmentEntity(int taskId, string userId)
    {
        return new TaskAssignmentEntity
        {
            TaskId = taskId,
            UserId = userId,
        };
    }
}
