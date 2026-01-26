using TodoList.Services.Database.Entities;
using TodoList.WebApi.Models.Models;

namespace TodoList.Services.Database.Helpers;

public static class TodoListMapper
{
    public static TodoListEntity ToEntityFromCreate(TodoListCreateModel model)
    {
        return new TodoListEntity
        {
            Title = model.Title,
            Description = model.Description,
            StartDate = model.StartDate,
        };
    }

    public static TodoListModel ToModel(TodoListEntity entity)
    {
        return new TodoListModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
        };
    }

    public static TodoListPreviewModel ToPreviewModel(TodoListEntity entity)
    {
        return new TodoListPreviewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Tasks = entity.Tasks != null ? entity.Tasks.Select(t => TaskMapper.ToModel(t)).ToList() : new List<TaskModel>(),
        };
    }

    public static void UpdateEntity(TodoListEntity entity, TodoListCreateModel model)
    {
        entity.Title = model.Title;
        entity.Description = model.Description;
        entity.StartDate = model.StartDate;
    }

    public static void UpdateEntity(TodoListEntity entity, TodoListUpdateModel model)
    {
        if (model.Title != null)
        {
            entity.Title = model.Title;
        }

        if (model.Description != null)
        {
            entity.Description = model.Description;
        }

        if (model.StartDate != null)
        {
            entity.StartDate = (DateTime)model.StartDate;
        }
    }
}
