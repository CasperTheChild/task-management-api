using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TodoList.Services.Enums;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Models;
using IAuthorizationService = TodoList.Services.Interfaces.IAuthorizationService;

namespace TodoList.WebApi.Controllers;

[Authorize]
[Route("api/TodoList/[controller]")]
[ApiController]
public class TaskAssignmentsController : ControllerBase
{
    private readonly ITaskAssignmentRepository service;
    private readonly IAuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;

    public TaskAssignmentsController(ITaskAssignmentRepository service, Services.Interfaces.IAuthorizationService authorizationService, ICurrentUserService currentUserService)
    {
        this.service = service;
        this.authorizationService = authorizationService;
        this.currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedModel<TaskModel>>> GetPagedAsync([FromQuery] AssignedTaskQuery query)
    {
        var models = await this.service.GetAssignedTasks(query);
        return Ok(models);
    }

    [HttpDelete("{taskId}/Users/{userId}")]
    public async Task<IActionResult> DeleteAsync(string userId, int taskId)
    {
        var currentUserId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(currentUserId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.service.RemoveTaskAssignmentAsync(userId, taskId);
        return NoContent();
    }

    [HttpPost("{taskId}/Users/{userId}")]
    public async Task<IActionResult> PostAsync(string userId, int taskId)
    {
        var currentUserId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditTasksAsync(currentUserId, taskId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        await this.service.AssignTaskToUserAsync(userId, taskId);
        return Created();
    }
}
