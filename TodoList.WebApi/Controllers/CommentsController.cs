using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Enums;
using TodoList.WebApi.Models.Models;
using IAuthorizationService = TodoList.Services.Interfaces.IAuthorizationService;

namespace TodoList.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository service;
    private readonly IAuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;

    public CommentsController(ICommentRepository service, IAuthorizationService authorizationService, ICurrentUserService currentUserService)
    {
        this.service = service;
        this.authorizationService = authorizationService;
        this.currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int taskId)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var comments = await this.service.GetAllComments(taskId, CommentRequestStatus.All);
        return Ok(comments);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedModel<CommentModel>>> GetPagedComments(int taskId, CommentRequestStatus status ,int pageNum = 1, int pageSize = 10)
    {
        

        var pagedComments = await this.service.GetPagedComments(taskId, status, pageNum, pageSize);
        return Ok(pagedComments);
    }

    [HttpGet("taskId")]
    public async Task<IActionResult> GetCommentById(int commentId)
    {
        var comment = await this.service.GetCommentById(commentId);
        
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(int taskId, [FromBody] CommentCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(userId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var createdComment = await this.service.CreateComment(taskId, model);
        return CreatedAtAction(nameof(GetCommentById), new { commentId = createdComment.Id }, createdComment);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.service.GetCommentById(commentId);

        if (entity == null)
        {
            return NotFound();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var updated = await this.service.UpdateComment(commentId, model);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeCommentStatus(int commentId, CommentStatus status)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.service.GetCommentById(commentId);

        if (entity == null)
        {
            return NotFound();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(userId, entity.TaskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var changed = await this.service.ChangeStatus(commentId, status);
        if (!changed)
        {
            return NotFound();
        }
        return NoContent();
    }

}
