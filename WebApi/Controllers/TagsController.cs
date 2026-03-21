using Application.DTOs;
using Application.Repository.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace WebApi.Controllers;

[ApiController]
[Route("api/TodoList/{todoListId}/[controller]")]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly TagService service;

    public TagsController(TagService service)
    {
        this.service = service;  
    }

    [HttpGet("allTags")]
    public async Task<ActionResult<IEnumerable<TagModel>>> GetAllTags(int todoListId)
    {
        var tags = await this.service.GetAllTags();
        return Ok(tags);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginatedModel<TagModel>>> GetUserTags(int todoListId, int pageNumber, int pageSize)
    {
        var tags = await this.service.GetTodoListTags(todoListId, pageNumber, pageSize);
        return Ok(tags);
    }

    [HttpGet("Id")]
    public async Task<ActionResult<TagModel>> GetTagById([FromQuery] int tagId)
    {
        var tag = await this.service.GetTagById(tagId);
        if (tag == null)
        {
            return NotFound();
        }
        return Ok(tag);
    }

    [HttpGet("Name")]
    public async Task<ActionResult<TagModel>> GetTagByName([FromQuery] string tagName)
    {
        var tag = await this.service.GetTagByName(tagName);
        if (tag == null)
        {
            return NotFound();
        }
        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(int todoListId, [FromBody] TagCreateModel model)
    {
        var createdTag = this.service.CreateTag(todoListId, model);
        return CreatedAtAction(nameof(CreateTag), new { id = createdTag.Id }, createdTag);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        await this.service.DeleteTag(tagId);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTag(int tagId, [FromBody] TagCreateModel model)
    {
        await this.service.UpdateTag(tagId, model);
        return NoContent();
    }
}
