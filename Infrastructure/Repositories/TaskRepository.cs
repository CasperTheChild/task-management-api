using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoListDbContext context;

    public TaskRepository(TodoListDbContext context, ICurrentUserService user)
    {
        this.context = context;
    }

    public void CreateAsync(int todoListId, TaskEntity entity)
    {
        this.context.Tasks.Add(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await this.context.Tasks.FindAsync(id);

        if (entity != null)
        {
            this.context.Remove(entity);
        }
    }

    public async Task<PaginatedModel<TaskEntity>> GetAllAsync(int todoListId, int pageNum, int pageSize)
    {
        var entity = await this.context.TodoLists.FindAsync(todoListId);

        var list = await this.context.Tasks.Where(t => t.TodoListId == todoListId).ToListAsync();

        var size = list.Count;

        return PaginationMapper.ToPaginatedEntity(list.Skip((pageNum - 1) * pageSize).Take(pageSize), size, pageNum, pageSize);
    }

    public async Task<IEnumerable<TaskEntity>> GetAllAsync(int todoListId)
    {
        return await this.context.Tasks.Where(t => t.TodoListId == todoListId).ToListAsync();
    }

    public async Task<TaskEntity?> GetAsync(int id)
    {
        return await this.context.Tasks.FindAsync(id);
    }

    public async Task UpdateAsync(int id, TaskEntity newEntity)
    {
        var entity = await this.context.Tasks.FindAsync(id);

        if (entity != null)
        {
            TaskMapper.UpdateEntity(entity, newEntity);
        }
    }
}
