using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationRepository = Application.Repository.Interfaces.IAuthorizationRepository;

namespace WebApi.Controllers;

[Authorize]
[Route("api/TodoList/{todoListId}/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly TaskService service;

    public TasksController(TaskService service)
    {
        this.service = service;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> GetByIdAsync(int todoListId, int id)
    {
        var model = await this.service.GetByIdAsync(todoListId, id);
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
        var model = await this.service.PatchAsync(todoListId, id, patchDoc);
        return Ok(model);
    }

    [HttpPost]
    public async Task<ActionResult<TaskModel>> PostAsync(int todoListId, TaskCreateModel model)
    {
        var res = await this.service.PostAsync(todoListId, model);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int todoListId, int id)
    {
        await this.service.DeleteAsync(todoListId, id);

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(int todoListId, int id, TaskCreateModel model)
    {
        await this.service.UpdateAsync(todoListId, id, model);
        return Ok();
    }
}
