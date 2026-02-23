using Application.DTOs;
using Application.Exceptions;
using Application.Repository.Interfaces;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace Application.Services;

public class CommentService
{
    private readonly ICommentRepository repository;
    private readonly ICurrentUserService currentUserService;
    private readonly IAuthorizationRepository authorizationRepository;

    public CommentService(ICommentRepository repository, ICurrentUserService currentuserService, IAuthorizationRepository authorizationRepository)
    {
        this.repository = repository;
        this.currentUserService = currentuserService;
        this.authorizationRepository = authorizationRepository;
    }

    public async Task<IEnumerable<CommentModel>> GetComments(int taskId)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var comments = await this.repository.GetAllComments(taskId, CommentRequestStatus.All);
        return comments;
    }

    public async Task<PaginatedModel<CommentModel>> GetPagedComments(int taskId, CommentRequestStatus status, int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var pagedComments = await this.repository.GetPagedComments(taskId, status, pageNum, pageSize);
        return pagedComments;
    }

    public async Task<CommentModel> GetComment(int commentId, int taskId)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanViewTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var comment = await this.repository.GetCommentById(commentId);

        if (comment == null)
        {
            throw new NotFoundException(nameof(CommentModel), commentId);
        }
        return comment;
    }

    public async Task<CommentModel> CreateComment(int taskId, CommentCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var createdComment = await this.repository.CreateComment(taskId, model);

        return createdComment;
    }

    public async Task UpdateComment(int commentId, CommentCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetCommentById(commentId);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CommentModel), commentId);
        }

        var permission = await this.authorizationRepository.CanEditTasksAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var updated = await this.repository.UpdateComment(commentId, model);
        if (!updated)
        {
            throw new NotFoundException(nameof(CommentModel), commentId);
        }
    }

    public async Task ChangeCommentStatus(int commentId, CommentStatus status)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.repository.GetCommentById(commentId);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CommentModel), commentId);
        }

        var permission = await this.authorizationRepository.CanEditTasksAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var changed = await this.repository.ChangeStatus(commentId, status);
        if (!changed)
        {
            throw new NotFoundException(nameof(CommentModel), commentId);
        }
    }

}
