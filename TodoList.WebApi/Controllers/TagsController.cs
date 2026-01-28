using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Models;
using IAuthorizationService = TodoList.Services.Interfaces.IAuthorizationService;

namespace TodoList.WebApi.Controllers;

[ApiController]
[Route("api/TodoList/[controller]")]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagRepository service;
    private readonly IAuthorizationService authorizationService;
    private readonly ICurrentUserService currentUserService;

    public TagsController(ITagRepository service, Services.Interfaces.IAuthorizationService authorizationService, ICurrentUserService currentUserService)
    {
        this.service = service;
        this.authorizationService = authorizationService;
        this.currentUserService = currentUserService;
    }

    [HttpGet("allTags")]
    public async Task<ActionResult<IEnumerable<TagModel>>> GetAllTags()
    {
        var tags = await this.service.GetAllTags();
        return Ok(tags);
    }

    [HttpGet("User/{userId}/paged")]
    public async Task<ActionResult<PaginatedModel<TagModel>>> GetUserTags(string userId, int pageNumber, int pageSize)
    {
        var tags = await this.service.GetUserTags(userId, pageNumber, pageSize);
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

    [HttpGet("TagId/{tagId}/paged")]
    public async Task<ActionResult<PaginatedModel<TaskModel>>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize)
    {

        var tasks = await this.service.GetPagedTasksByTag(tagId, pageNumber, pageSize);
        return Ok(tasks);
    }

    [HttpGet("TaskId/{taskId}/paged")]
    public async Task<ActionResult<PaginatedModel<TagModel>>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize)
    {
        var tags = await this.service.GetPagedTagsByTask(taskId, pageNumber, pageSize);
        return Ok(tags);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateModel model)
    {
        var createdTag = await this.service.CreateTag(model);
        return CreatedAtAction(nameof(CreateTag), new { id = createdTag.Id }, createdTag);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        var result = await this.service.DeleteTag(tagId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTag(int tagId, [FromBody] TagCreateModel model)
    {
        var updatedTag = await this.service.UpdateTag(tagId, model);
        if (updatedTag == null)
        {
            return NotFound();
        }
        return Ok(updatedTag);
    }

    [HttpPost("Assign")]
    public async Task<IActionResult> AssignTagToTask(int taskId, int tagId)
    {
        var result = await this.service.AssignTag(taskId, tagId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("Remove")]
    public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
    {
        var result = await this.service.RemoveTag(taskId, tagId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

}
