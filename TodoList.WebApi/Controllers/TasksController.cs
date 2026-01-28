using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Models;
using IAuthorizationService = TodoList.Services.Interfaces.IAuthorizationService;

namespace TodoList.WebApi.Controllers;

[Authorize]
[Route("api/TodoList/{todoListId}/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository service;
    private readonly IAuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;

    public TasksController(ITaskRepository service, Services.Interfaces.IAuthorizationService authorizationService, ICurrentUserService currentUserService)
    {
        this.service = service;
        this.authorizationService = authorizationService;
        this.currentUserService = currentUserService;

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> GetByIdAsync(int todoListId, int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewTasksAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var model =  await this.service.GetAsync(todoListId, id);

        if (model == null)
        {
            return NotFound();
        }

        return Ok(model);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedModel<TaskModel>>> GetPagedAsync(int todoListId, int pageNum, int pageSize)
    {
        var models = await this.service.GetAllAsync(todoListId, pageNum, pageSize);

        return Ok(models);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllAsync(int todoListId)
    {
        var models = await this.service.GetAllAsync(todoListId);

        return Ok(models);
    }

    [HttpPatch("{id}")]
    [Consumes("application/json-patch+json")]
    public async Task<ActionResult<TaskModel>> PatchAsync(int todoListId, int id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TaskUpdateModel> patchDoc)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.service.Patch(todoListId, id, patchDoc);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<TaskModel>> PostAsync(int todoListId, TaskCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await this.service.CreateAsync(todoListId, model);

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int todoListId, int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.service.DeleteAsync(todoListId, id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult<bool>> UpdateAsync(int todoListId, int id, TaskCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, todoListId);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.service.UpdateAsync(todoListId, id, model);

        if (!success)
        {
            return NotFound();
        }

        return Ok();
    }
}
