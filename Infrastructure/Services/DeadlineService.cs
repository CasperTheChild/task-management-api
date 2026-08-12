using Application.Repository.Interfaces;
using Application.Services.Interfaces;

namespace Infrastructure.Services;

public class DeadlineService : IDeadlineService
{
    private readonly ITaskRepository taskRepository;
    private readonly INotificationService notificationService;

    public DeadlineService(ITaskRepository taskRepository, INotificationService notificationService)
    {
        this.taskRepository = taskRepository;
        this.notificationService = notificationService;
    }

    public async Task CheckUpcomingDeadlinesAsync()
    {
        // Get the current time
        var now = DateTime.UtcNow;
        // Get tasks due in the next 61 minutes
        var tasksDueSoon = await this.taskRepository.GetTasksDueBetween(now, now.AddMinutes(61));
        foreach (var task in tasksDueSoon)
        {
            foreach (var user in task.AssignedUsers)
            {
                // Send notification to each assigned user
                await this.notificationService.SendNotificationAsync(
                user.UserId,
                "Task Deadline Approaching",
                $"Your task '{task.Title}' is due on {task.EndDate}. Please make sure to complete it on time.",
                false);
            }
        }
    }
}
