using Microsoft.AspNetCore.Mvc;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = TodoList.Services.Interfaces.IAuthorizationService;

namespace TodoList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListRepository service;
    private readonly IAuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;

    public TodoListController(ITodoListRepository service, IAuthorizationService authorizationService, ICurrentUserService currentUserService)
    {
        this.service = service;
        this.authorizationService = authorizationService;
        this.currentUserService = currentUserService;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListModel>>> GetAll()
    {
        var userId = this.currentUserService.UserId;

        var models = await this.service.GetAllAsync();

        return Ok(models);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedModel<TodoListModel>>> GetPaginated(int pageNum, int pageSize)
    {
        var models = await this.service.GetAllAsync(pageNum, pageSize);

        return Ok(models);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListModel>> GetById(int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanViewAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var model = await this.service.GetAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return Ok(model);
    }

    [HttpGet("preview")]
    public async Task<ActionResult<PaginatedModel<TodoListPreviewModel>>> GetAllPreviewPaginated(int pageNum, int pageSize)
    {
        var models = await this.service.GetAllPreviewAsync(pageNum, pageSize);

        return Ok(models);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoListCreateModel model)
    {
        var newModel = await this.service.CreateAsync(model);
        return CreatedAtAction(nameof(GetAll), new {Id =  newModel.Id}, newModel);
    }

    [HttpPatch("{id}")]
    [Consumes("application/json-patch+json")]
    public async Task<ActionResult<TodoListModel>> Patch(int id, JsonPatchDocument<TodoListUpdateModel> patchDoc)
    {

        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var updatedModel = await this.service.PatchAsync(id, patchDoc);
        if (updatedModel == null)
        {
            return NotFound();
        }
        return Ok(updatedModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TodoListCreateModel model)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.service.UpdateAsync(id, model);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = this.currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var permission = await this.authorizationService.CanEditAsync(userId, id);

        if (!permission)
        {
            throw new UnauthorizedAccessException();
        }

        var success = await this.service.DeleteAsync(id);
        if (success)
        {
            return NoContent();
        }

        return NotFound();
    }
}
