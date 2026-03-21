using Application.DTOs;
using Application.Repository.Interfaces;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly TodoListDbContext context;

    public SearchRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<TaskModel>> SearchAsync(SearchParameterModel model)
    {
        var query = this.context.Tasks.Where(t => t.Title.Contains(model.Text ?? string.Empty));

        if (model.Filter.TodoListId.HasValue)
        {
            query = query.Where(t => t.TodoListId == model.Filter.TodoListId);
        }

        if (model.Filter.TagId.HasValue)
        {
            query = query.Where(t => t.TaskTags.Any(tag => tag.TagId == model.Filter.TagId));
        }

        if (model.Filter.TaskStatus.HasValue)
        {
            if (model.Filter.TaskStatus == Domain.Enums.TaskStatusFilter.Completed)
            {
                query = query.Where(t => t.IsCompleted);
            }
            else if (model.Filter.TaskStatus == Domain.Enums.TaskStatusFilter.Ongoing)
            {
                query = query.Where(t => !t.IsCompleted);
            }
        }

        if (model.Filter.DueFrom.HasValue)
        {
            query = query.Where(t => t.EndDate >= model.Filter.DueFrom);
        }

        if (model.Filter.DueTo.HasValue)
        {
            query = query.Where(t => t.EndDate <= model.Filter.DueTo);
        }

        query = model.Sort.SortOption switch
        {
            Domain.Enums.TaskSortOption.CreatedAtAsc => query.OrderBy(t => t.StartDate),
            Domain.Enums.TaskSortOption.CreatedAtDesc => query.OrderByDescending(t => t.StartDate),
            Domain.Enums.TaskSortOption.DueDateAsc => query.OrderBy(t => t.EndDate),
            Domain.Enums.TaskSortOption.DueDateDesc => query.OrderByDescending(t => t.EndDate),
            Domain.Enums.TaskSortOption.TitleAsc => query.OrderBy(t => t.Title),
            Domain.Enums.TaskSortOption.TitleDesc => query.OrderByDescending(t => t.Title),
            _ => query
        };

        return await query.Select(t => TaskMapper.ToModel(t)).ToListAsync();
    }

    public async Task<PaginatedModel<TaskModel>> SearchPagedAsync(SearchParameterModel model, int pageNum, int pageSize)
    {
        var query = this.context.Tasks.Where(t => t.Title.Contains(model.Text ?? string.Empty));

        if (model.Filter.TodoListId.HasValue)
        {
            query = query.Where(t => t.TodoListId == model.Filter.TodoListId);
        }

        if (model.Filter.TagId.HasValue)
        {
            query = query.Where(t => t.TaskTags.Any(tag => tag.TagId == model.Filter.TagId));
        }

        if (model.Filter.TaskStatus.HasValue)
        {
            if (model.Filter.TaskStatus == Domain.Enums.TaskStatusFilter.Completed)
            {
                query = query.Where(t => t.IsCompleted);
            }
            else if (model.Filter.TaskStatus == Domain.Enums.TaskStatusFilter.Ongoing)
            {
                query = query.Where(t => !t.IsCompleted);
            }
        }

        if (model.Filter.DueFrom.HasValue)
        {
            query = query.Where(t => t.EndDate >= model.Filter.DueFrom);
        }

        if (model.Filter.DueTo.HasValue)
        {
            query = query.Where(t => t.EndDate <= model.Filter.DueTo);
        }

        query = model.Sort.SortOption switch
        {
            Domain.Enums.TaskSortOption.CreatedAtAsc => query.OrderBy(t => t.StartDate),
            Domain.Enums.TaskSortOption.CreatedAtDesc => query.OrderByDescending(t => t.StartDate),
            Domain.Enums.TaskSortOption.DueDateAsc => query.OrderBy(t => t.EndDate),
            Domain.Enums.TaskSortOption.DueDateDesc => query.OrderByDescending(t => t.EndDate),
            Domain.Enums.TaskSortOption.TitleAsc => query.OrderBy(t => t.Title),
            Domain.Enums.TaskSortOption.TitleDesc => query.OrderByDescending(t => t.Title),
            _ => query
        };

        var totalItems = await query.CountAsync();

        query = query.Skip((pageNum - 1) * pageSize).Take(pageSize);

        var result = await query.Select(t => TaskMapper.ToModel(t)).ToListAsync();

        return PaginationMapper.ToPaginatedModel(result, pageNum, pageSize, totalItems);
    }
}
