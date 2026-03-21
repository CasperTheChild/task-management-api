using Application.DTOs;
using Domain.Entities;

namespace Application.Helpers;

public static class PaginationMapper
{
    public static PaginatedModel<TodoListEntity> ToPaginatedEntity(IEnumerable<TodoListEntity> entities, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TodoListEntity>
        {
            Items = entities,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<TaskEntity> ToPaginatedEntity(IEnumerable<TaskEntity> entities, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TaskEntity>
        {
            Items = entities,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<CommentEntity> ToPaginatedEntity(IEnumerable<CommentEntity> entities, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<CommentEntity>
        {
            Items = entities,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<TagEntity> ToPaginatedEntity(IEnumerable<TagEntity> entities, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TagEntity>
        {
            Items = entities,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<TodoListModel> ToPaginatedModel(IEnumerable<TodoListModel> models, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TodoListModel>
        {
            Items = models,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<TodoListPreviewModel> ToPaginatedModel(IEnumerable<TodoListPreviewModel> models, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TodoListPreviewModel>
        {
            Items = models,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<TodoListModel> ToModelFromEntity(PaginatedModel<TodoListEntity> paginatedEntity)
    {
        return new PaginatedModel<TodoListModel>
        {
            Items = paginatedEntity.Items.Select(TodoListMapper.ToModel).ToList(),
            TotalItems = paginatedEntity.TotalItems,
            ItemsPerPage = paginatedEntity.ItemsPerPage,
            CurrentPage = paginatedEntity.CurrentPage,
        };
    }

    public static PaginatedModel<TaskModel> ToPaginatedModel(IEnumerable<TaskModel> models, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TaskModel>
        {
            Items = models,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<UserSummaryModel> ToPaginatedModel(IEnumerable<UserSummaryModel> models, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<UserSummaryModel>
        {
            Items = models,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<TagModel> ToPaginatedModel(IEnumerable<TagModel> models, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<TagModel>
        {
            Items = models,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }

    public static PaginatedModel<CommentModel> ToPaginatedModel(IEnumerable<CommentModel> models, int totalItems, int pageNum, int pageSize)
    {
        return new PaginatedModel<CommentModel>
        {
            Items = models,
            TotalItems = totalItems,
            ItemsPerPage = pageSize,
            CurrentPage = pageNum,
        };
    }
}
