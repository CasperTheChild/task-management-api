using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Helpers;

public static class CommentMapper
{
    public static CommentEntity ToEntity(int taskId, CommentCreateModel model, string userId)
    {
        return new CommentEntity
        {
            TaskId = taskId,
            Content = model.Content,
            Status = CommentStatus.Active,
            UserId = userId,
        };
    }

    public static CommentModel ToModel(CommentEntity entity)
    {
        return new CommentModel
        {
            Id = entity.Id,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt,
            TaskId = entity.TaskId,
            UserId = entity.UserId,
            Status = entity.Status,
        };
    }

    public static void UpdateStatus(CommentEntity entity, CommentStatus status)
    {
        entity.Status = status;
    }

    public static void UpdateEntity(CommentEntity entity, CommentCreateModel model)
    {
        entity.Content = model.Content;
    }
}
