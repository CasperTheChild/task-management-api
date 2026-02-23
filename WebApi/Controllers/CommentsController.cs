using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IAuthorizationRepository = Application.Repository.Interfaces.IAuthorizationRepository;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly CommentService commentService;

    public CommentsController(CommentService commentService)
    {
        this.commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentModel>>> GetComments(int taskId)
    {
        var comments = await commentService.GetComments(taskId);
        return Ok(comments);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedModel<CommentModel>>> GetPagedComments(int taskId, CommentRequestStatus status ,int pageNum = 1, int pageSize = 10)
    {
        var comments = await this.commentService.GetPagedComments(taskId, status, pageNum, pageSize);
        return Ok(comments);
    }

    [HttpGet("taskId/{taskId}")]
    public async Task<IActionResult> GetCommentById(int commentId, int taskId)
    {
        var comment = await this.commentService.GetComment(commentId, taskId);

        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<CommentModel>> CreateComment(int taskId, [FromBody] CommentCreateModel model)
    {
        var comment = this.commentService.CreateComment(taskId, model);

        return Ok(comment);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentCreateModel model)
    {
        _ = this.commentService.UpdateComment(commentId, model);
        return Ok();
    }

    [HttpPut("status")]
    public async Task<IActionResult> ChangeCommentStatus(int commentId, CommentStatus status)
    {
        _ = this.commentService.ChangeCommentStatus(commentId, status);
        return Ok();
    }
}
