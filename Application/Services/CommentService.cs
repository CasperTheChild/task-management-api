using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
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
    private readonly ITaskRepository taskRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly AuthorizationService authorizationRepository;
    private readonly IUnitOfWork unitOfWork;

    public CommentService(ICommentRepository repository, ICurrentUserService currentuserService, AuthorizationService authorizationRepository, ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.currentUserService = currentuserService;
        this.authorizationRepository = authorizationRepository;
        this.taskRepository = taskRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentModel>> GetComments(int taskId)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var taskEntity = await this.taskRepository.GetAsync(taskId);

        if (taskEntity == null)
        {
            throw new NotFoundException(nameof(TaskModel), taskId);
        }

        var permission = await this.authorizationRepository.CanViewAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entities = await this.repository.GetAllComments(taskId, CommentRequestStatus.All);

        return entities.Select(e => CommentMapper.ToModel(e));
    }

    public async Task<PaginatedModel<CommentModel>> GetPagedComments(int taskId, CommentRequestStatus status, int pageNum, int pageSize)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var taskEntity = await this.taskRepository.GetAsync(taskId);

        if (taskEntity == null)
        {
            throw new NotFoundException(nameof(TaskModel), taskId);
        }

        var permission = await this.authorizationRepository.CanViewAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entities = await this.repository.GetPagedComments(taskId, status, pageNum, pageSize);

        var models = entities.Items.Select(e => CommentMapper.ToModel(e));

        return new PaginatedModel<CommentModel>
        {
            Items = models,
            TotalItems = entities.TotalItems,
            ItemsPerPage = entities.ItemsPerPage,
            CurrentPage = entities.CurrentPage,
        };
    }

    public async Task<CommentModel> GetComment(int commentId)
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

        var permission = await this.authorizationRepository.CanViewAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        return CommentMapper.ToModel(entity);
    }

    public async Task<CommentModel> CreateComment(int taskId, CommentCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationRepository.CanEditAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = CommentMapper.ToEntity(taskId, model, userId);

        this.repository.CreateComment(taskId, entity);

        await this.unitOfWork.SaveChangesAsync();

        return CommentMapper.ToModel(entity);
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

        var permission = await this.authorizationRepository.CanEditAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.UpdateComment(commentId, CommentMapper.ToEntity(entity.TaskId, model, userId));

        await this.unitOfWork.SaveChangesAsync();
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

        var permission = await this.authorizationRepository.CanEditAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.repository.ChangeStatus(commentId, status);

        await this.unitOfWork.SaveChangesAsync();
    }
}
