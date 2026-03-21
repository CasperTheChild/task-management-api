using Application.Services;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    [Route("api/TodoList/{todoListId}/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthorizationService service;

        public AuthorizationController(AuthorizationService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role)
        {
            await this.service.AssignRoleAsync(todoListId, targetUserId, role);
            return NoContent();
        }
    }
}
