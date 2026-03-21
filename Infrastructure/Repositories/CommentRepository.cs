using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;
using Infrastructure.Context;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly TodoListDbContext context;

    private readonly ICurrentUserService user;

    public CommentRepository(TodoListDbContext context, ICurrentUserService user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task ChangeStatus(int commentId, CommentStatus status)
    {
        var entity = await this.context.Comments.FindAsync(commentId);

        if (entity != null)
        {
            entity.Status = status;
        }
    }

    public void CreateComment(int taskId, CommentEntity entity)
    {
        this.context.Comments.Add(entity);
    }

    public async Task<IEnumerable<CommentEntity>> GetAllComments(int taskId, CommentRequestStatus status)
    {
        return await this.context.Comments.Where(c => c.TaskId == taskId && (status == CommentRequestStatus.All || c.Status == (CommentStatus)status)).ToListAsync();
    }

    public async Task<CommentEntity?> GetCommentById(int commentId)
    {
        return await this.context.Comments.FindAsync(commentId);
    }

    public async Task<PaginatedModel<CommentEntity>> GetPagedComments(int taskId, CommentRequestStatus status, int pageNum, int pageSize)
    {
        var query = this.context.Comments.Where(c => c.TaskId == taskId && (status == CommentRequestStatus.All || c.Status == (CommentStatus)status));
        var totalItems = await query.CountAsync();
        var items = await query.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginationMapper.ToPaginatedEntity(items, totalItems, pageNum, pageSize);
    }

    public async Task UpdateComment(int commentId, CommentEntity newEntity)
    {
        var entity = await this.context.Comments.FindAsync(commentId);
        if (entity != null)
        {
            CommentMapper.UpdateEntity(entity, newEntity);
        }
    }
}
