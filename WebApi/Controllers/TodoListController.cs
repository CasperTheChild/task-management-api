using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Application.Repository.Interfaces;
using IAuthorizationRepository = Application.Repository.Interfaces.IAuthorizationRepository;
using Application.DTOs;
using Application.Services.Interfaces;
using Application.Services;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly TodoListService service;

    public TodoListController(TodoListService service) 
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListModel>>> GetAll()
    {
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
        var model = await this.service.GetByIdAsync(id);
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
        var createdModel = await this.service.CreateAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = createdModel.Id }, createdModel);
    }

    [HttpPatch("{id}")]
    [Consumes("application/json-patch+json")]
    public async Task<ActionResult<TodoListModel>> Patch(int id, JsonPatchDocument<TodoListUpdateModel> patchDoc)
    {
        var patchedModel = await this.service.PatchAsync(id, patchDoc);
        if (patchedModel == null)
        {
            return NotFound();
        }
        return Ok(patchedModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TodoListCreateModel model)
    {
        var success = await this.service.UpdateAsync(id, model);
        if (success)
        {
            return NoContent();
        }
        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await this.service.DeleteAsync(id);
        if (success)
        {
            return NoContent();
        }
        return NotFound();
    }
}
