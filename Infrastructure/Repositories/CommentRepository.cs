using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> ChangeStatus(int commentId, CommentStatus status)
    {
        var entity = await this.context.Comments.FindAsync(commentId);

        if (entity == null)
        {
            return false;
        }

        if (entity.Status == status)
        {
            return true;
        }

        CommentMapper.UpdateStatus(entity, status);

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<CommentModel> CreateComment(int taskId, CommentCreateModel model)
    {
        var userId = this.user.UserId ?? throw new InvalidOperationException("User ID cannot be null.");

        var entity = CommentMapper.ToEntity(taskId, model, userId);

        await this.context.AddAsync(entity);

        await this.context.SaveChangesAsync();

        var res = CommentMapper.ToModel(entity);

        return res;
    }

    public async Task<IEnumerable<CommentModel>> GetAllComments(int taskId, CommentRequestStatus status)
    {
        var query = this.context.Tasks.Where(t => t.Id == taskId).SelectMany(t => t.Comments);

        if (status == CommentRequestStatus.Active)
        {
            query = query.Where(c => c.Status == CommentStatus.Active);
        }
        else if (status == CommentRequestStatus.Resolved)
        {
            query = query.Where(c => c.Status == CommentStatus.Resolved);
        }

        var comments = await query.Select(c => CommentMapper.ToModel(c)).ToListAsync();

        return comments;
    }

    public async Task<CommentModel?> GetCommentById(int commentId)
    {
        var comment = await this.context.Comments.FindAsync(commentId);

        if (comment == null)
        {
            return null;
        }

        var result = CommentMapper.ToModel(comment);

        return result;
    }

    public async Task<PaginatedModel<CommentModel>> GetPagedComments(int taskId, CommentRequestStatus status, int pageNum, int pageSize)
    {
        var query = this.context.Tasks.Where(t => t.Id == taskId).SelectMany(t => t.Comments);

        if (status == CommentRequestStatus.Active)
        {
            query = query.Where(c => c.Status == CommentStatus.Active);
        }
        else if (status == CommentRequestStatus.Resolved)
        {
            query = query.Where(c => c.Status == CommentStatus.Resolved);
        }

        var totalItems = query.Count();

        query = query.Skip((pageNum - 1) * pageSize).Take(pageSize);

        var comments = query.Select(c => CommentMapper.ToModel(c)).ToList();

        var result = PaginationMapper.ToPaginatedModel(comments, totalItems, pageNum, pageSize);

        return result;
    }

    public async Task<bool> UpdateComment(int commentId, CommentCreateModel model)
    {
        var entity = this.context.Comments.SingleOrDefault(t => t.Id == commentId);

        if (entity == null)
        {
            return false;
        }

        CommentMapper.UpdateEntity(entity, model);

        await this.context.SaveChangesAsync();

        return true;
    }
}
