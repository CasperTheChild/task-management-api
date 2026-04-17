using Application.DTOs;
using Domain.Enums;

namespace Application.Repository.Interfaces;

public interface ITaskAssignmentRepository
{
    Task<PaginatedModel<TaskModel>> GetAssignedTasks(AssignedTaskQuery query);

    void AssignTaskToUserAsync(string userId, int taskId);

    Task RemoveTaskAssignmentAsync(string userId, int taskId);
}
