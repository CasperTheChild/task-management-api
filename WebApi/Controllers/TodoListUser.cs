using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoListUser : ControllerBase
{
    private readonly TodoListUserService service;

    public TodoListUser(TodoListUserService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AssignRoleAsync(int todoListId, string userId, TodoListRole role)
    {
        await this.service.AssignRoleAsync(todoListId, userId, role);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveRoleAsync(int todoListId, string userId)
    {
        await this.service.RemoveRoleAsync(todoListId, userId);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> HasRoleAsync(int todoListId, string userId)
    {
        var role = await this.service.HasRoleAsync(todoListId, userId);
        return Ok(role);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoleAsync(int todoListId, string userId, TodoListRole role)
    {
        await this.service.UpdateRoleAsync(todoListId, userId, role);
        return Ok();
    }
}
