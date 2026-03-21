using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext context;

    public TodoListRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public void Create(TodoListEntity entity)
    {
        this.context.TodoLists.Add(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await this.context.TodoLists.FindAsync(id);

        if (entity != null)
        {
            this.context.TodoLists.Remove(entity);
        }
    }

    public async Task<PaginatedModel<TodoListEntity>?> GetAllAsync(string userId, int pageNum, int pageSize)
    {
        var query = this.context.TodoLists
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId));

        var totalItems = await query.CountAsync();

        var entities = await query.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        return PaginationMapper.ToPaginatedEntity(entities, totalItems, pageNum, pageSize);
    }

    public async Task<IEnumerable<TodoListEntity>?> GetAllAsync(string userId)
    {
        var entities = await this.context.TodoLists
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .ToListAsync();

        return entities;
    }

    public async Task<PaginatedModel<TodoListPreviewModel>> GetAllPreviewAsync(string userId, int pageNum, int pageSize, int amount)
    {
        var query = this.context.TodoLists
            .Where(t => t.Members.Any(m => m.UserId == userId));

        var totalItems = await query.CountAsync();

        var result = await query
            .Include(t => t.Tasks)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(t => TodoListMapper.ToPreviewModel(t))
            .ToListAsync();

        return PaginationMapper.ToPaginatedModel(result, totalItems, pageNum, pageSize);
    }

    public async Task<TodoListEntity?> GetAsync(int id)
    {
        var entity = await this.context.TodoLists.FindAsync(id);

        return entity;
    }

    public async Task UpdateAsync(int id, TodoListEntity newEntity)
    {
        var entity = await this.context.TodoLists.FindAsync(id);

        if (entity != null)
        {
            TodoListMapper.UpdateEntity(entity, newEntity);
        }
    }
}
