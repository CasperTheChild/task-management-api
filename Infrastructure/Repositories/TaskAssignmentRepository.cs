using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskAssignmentRepository : ITaskAssignmentRepository
{
    private readonly TodoListDbContext context;
    private readonly ICurrentUserService user;

    public TaskAssignmentRepository(TodoListDbContext context, ICurrentUserService user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<PaginatedModel<TaskModel>> GetAssignedTasks(AssignedTaskQuery options)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        IQueryable<TaskAssignmentEntity> query = this.context.TaskAssignments.Where(t => t.UserId == userId);

        query = options.StatusFilter switch
        {
            TaskStatusFilter.All => query,
            TaskStatusFilter.Completed => query.Where(t => t.Task.IsCompleted),
            TaskStatusFilter.Ongoing => query.Where(t => !t.Task.IsCompleted),
            _ => query
        };

        query = options.SortOption switch
        {
            TaskSortOption.CreatedAtAsc => query.OrderBy(t => t.Task.StartDate),
            TaskSortOption.CreatedAtDesc => query.OrderByDescending(t => t.Task.StartDate),
            TaskSortOption.DueDateAsc => query.OrderBy(t => t.Task.EndDate == null).ThenBy(t => t.Task.EndDate),
            TaskSortOption.DueDateDesc => query.OrderByDescending(t => t.Task.EndDate == null).ThenByDescending(t => t.Task.EndDate),
            _ => query
        };

        var totalItems = await query.CountAsync();

        query = query.Skip((options.PageNum - 1) * options.PageSize).Take(options.PageSize).Include(t => t.Task);

        var entities = await query.ToListAsync();

        var models = entities.Select(t => TaskMapper.ToModel(t.Task));

        return PaginationMapper.ToPaginatedModel(models, totalItems, options.PageNum, options.PageSize);
    }

    public async Task AssignTaskToUserAsync(string userId, int taskId)
    {
        var entity = await this.context.TaskAssignments.Where(t => t.TaskId == taskId && t.UserId == userId).FirstOrDefaultAsync();

        if (entity != null)
        {
            throw new InvalidOperationException("Task is already assigned to the user.");
        }

        var taskAssignmentEntity = TaskAssignmentMapper.ToTaskAssignmentEntity(taskId, userId);

        this.context.TaskAssignments.Add(taskAssignmentEntity);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveTaskAssignmentAsync(string userId, int taskId)
    {
        var entity = await this.context.TaskAssignments.Where(t => t.TaskId == taskId && t.UserId == userId).Include(t => t.Task).FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new InvalidOperationException("Task assignment not found for the user.");
        }

        if (entity.UserId != userId)
        {
            throw new InvalidOperationException("Only the owner can remove the task assignment.");
        }

        this.context.TaskAssignments.Remove(entity);

        await this.context.SaveChangesAsync();
    }
}
