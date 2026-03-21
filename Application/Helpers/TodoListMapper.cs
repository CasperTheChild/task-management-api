using Application.DTOs;
using Domain.Entities;

namespace Application.Helpers;

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

    public static void ToEntityFromUpdate(TodoListEntity entity, TodoListUpdateModel model)
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

    public static void UpdateEntity(TodoListEntity entity, TodoListEntity updatedEntity)
    {
        entity.Title = updatedEntity.Title;
        entity.Description = updatedEntity.Description;
        entity.StartDate = updatedEntity.StartDate;
    }
}
