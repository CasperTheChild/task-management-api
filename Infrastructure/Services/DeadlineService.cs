using Application.Repository.Interfaces;
using Application.Services.Interfaces;

namespace Infrastructure.Services;

public class DeadlineService : IDeadlineService
{
    private readonly ITaskRepository taskRepository;
    private readonly INotificationService notificationService;

    public async Task CheckDeadlines(int userId)
    {
        // Get the current time
        var now = DateTime.UtcNow;
        // Get tasks due in the next 24 hours
        var tasksDueSoon = await this.taskRepository.GetTasksDueBetween(userId.ToString(), now, now.AddHours(24));
        foreach (var task in tasksDueSoon)
        {
            // Send notification for each task due soon
            await this.notificationService.SendNotificationAsync(
                "user",
                "Task Deadline Approaching",
                $"Your task '{task.Title}' is due on {task.EndDate}. Please make sure to complete it on time.",
                false);
        }
    }
}
