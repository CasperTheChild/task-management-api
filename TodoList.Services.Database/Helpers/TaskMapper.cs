using TodoList.Services.Database.Entities;
using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Database.Helpers;

public static class TaskMapper
{
    public static TaskEntity ToEntityFromCreate(int todoListId, TaskCreateModel model)
    {
        return new TaskEntity
        {
            Title = model.Title,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            IsCompleted = model.IsCompleted,
            TodoListId = todoListId,
        };
    }

    public static TaskModel ToModel(TaskEntity entity)
    {
        return new TaskModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            IsCompleted = entity.IsCompleted,
            TodoListId = entity.TodoListId,
        };
    }

    public static void UpdateEntity(TaskEntity entity, TaskCreateModel model)
    {
        entity.Title = model.Title;
        entity.Description = model.Description;
        entity.StartDate = model.StartDate;
        entity.EndDate = model.EndDate;
        entity.IsCompleted = model.IsCompleted;
    }

    public static void UpdateEntity(TaskEntity entity, TaskUpdateModel model)
    {
        if (model.Title != null)
        {
            entity.Title = model.Title;
        }

        if (model.Description != null)
        {
            entity.Description = model.Description;
        }

        if (entity.EndDate != null)
        {
            entity.EndDate = model.EndDate;
        }

        if (model.StartDate != null)
        {
            entity.StartDate = (DateTime)model.StartDate;
        }

        if (model.IsCompleted != null)
        {
            entity.IsCompleted = (bool)model.IsCompleted;
        }
    }
}
