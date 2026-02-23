using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using IAuthorizationRepository = Application.Repository.Interfaces.IAuthorizationRepository;

namespace WebApi.Controllers;

[Authorize]
[Route("api/TodoList/[controller]")]
[ApiController]
public class TaskAssignmentsController : ControllerBase
{
    private readonly TaskAssignmentService service;

    public TaskAssignmentsController(TaskAssignmentService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedModel<TaskModel>>> GetPagedAsync([FromQuery] AssignedTaskQuery query)
    {
        var models = await this.service.GetPagedAsync(query);
        return Ok(models);
    }

    [HttpDelete("{taskId}/Users/{userId}")]
    public async Task<IActionResult> DeleteAsync(string userId, int taskId)
    {
        _ = this.service.DeleteAsync(userId, taskId);
        return NoContent();
    }

    [HttpPost("{taskId}/Users/{userId}")]
    public async Task<IActionResult> PostAsync(string userId, int taskId)
    {
        _ = this.service.PostAsync(userId, taskId);
        return Created();
    }
}
