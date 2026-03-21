using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Repository.Interfaces;

public interface ICommentRepository
{
    public Task<IEnumerable<CommentEntity>> GetAllComments(int taskId, CommentRequestStatus status);

    public Task<PaginatedModel<CommentEntity>> GetPagedComments(int taskId, CommentRequestStatus status, int pageNum, int pageSize);

    public Task<CommentEntity> GetCommentById(int commentId);

    public void CreateComment(int taskId, CommentEntity entity);

    public Task UpdateComment(int commentId, CommentEntity newEntity);

    public Task ChangeStatus(int commentId, CommentStatus status);
}
