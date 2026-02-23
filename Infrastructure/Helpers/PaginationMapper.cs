using Application.DTOs;

namespace Infrastructure.Helpers;

public static class PaginationMapper
{
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
