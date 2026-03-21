using Application.DTOs;
using Domain.Entities;

namespace Application.Helpers;

public static class TagMapper
{
    public static TagModel ToModel(TagEntity entity)
    {
        return new TagModel
        {
            Id = entity.Id,
            Name = entity.TagName,
            TodoListId = entity.TodoListId,
        };
    }

    public static TagEntity ToEntity(TagModel model)
    {
        return new TagEntity
        {
            Id = model.Id,
            TagName = model.Name,
            NormalizedTagName = model.Name.ToLower(),
            TodoListId = model.TodoListId,
        };
    }

    public static TagEntity ToEntity(int todoListId, TagCreateModel model)
    {
        return new TagEntity
        {
            TagName = model.Name,
            NormalizedTagName = model.Name.ToLower(),
            TodoListId = todoListId,
        };
    }

    public static TaskTagEntity ToTaskTagEntity(int taskId, int tagId)
    {
        return new TaskTagEntity
        {
            TaskId = taskId,
            TagId = tagId,
        };
    }
}
