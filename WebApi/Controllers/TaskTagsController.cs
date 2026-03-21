using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTagsController : ControllerBase
    {
        private readonly TaskTagService service;

        public TaskTagsController(TaskTagService service)
        {
            this.service = service;
        }

        [HttpGet("TaskId/{taskId}/paged")]
        public async Task<ActionResult<PaginatedModel<TagModel>>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize)
        {
            var tags = await this.service.GetPagedTagsByTask(taskId, pageNumber, pageSize);
            return Ok(tags);
        }

        [HttpGet("TagId/{tagId}/paged")]
        public async Task<ActionResult<PaginatedModel<TaskModel>>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize)
        {
            var tasks = await this.service.GetPagedTasksByTag(tagId, pageNumber, pageSize);
            return Ok(tasks);
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> AssignTag(int taskId, int tagId)
        {
            this.service.AssignTag(taskId, tagId);
            return NoContent();
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveTag(int taskId, int tagId)
        {
            await this.service.RemoveTag(taskId, tagId);
            return NoContent();
        }
    }
}
