using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TodoList.Services.Interfaces;
using TodoList.WebApi.Models.Enums;
using IAuthorizationService = TodoList.Services.Interfaces.IAuthorizationService;

namespace TodoList.WebApi.Controllers
{
    [Route("api/TodoList/{todoListId}/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService service;
        private readonly ICurrentUserService currentUserService;

        public AuthorizationController(IAuthorizationService service, ICurrentUserService currentUserService)
        {
            this.service = service;
            this.currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleAsync(int todoListId, string targetUserId, TodoListRole role)
        {
            string? userId = this.currentUserService.UserId;

            if (userId == null)
            {
                throw new UnauthorizedAccessException("UserId is not found!");
            }

            var canAssign = await this.service.CanAssignRoleAsync(userId, todoListId);

            if (!canAssign)
            {
                throw new UnauthorizedAccessException("The current userId can not assign roles");
            }

            var success = await this.service.AssignRoleAsync(todoListId, targetUserId, role);

            if (!success)
            {
                return BadRequest("Couldn't assign");
            }

            return Ok();
        }
    }
}
