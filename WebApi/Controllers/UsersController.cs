using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/TodoList/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository repository;

    public UsersController(IUserRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet("exists/email/{email}")]
    public async Task<ActionResult<bool>> ExistsByEmailAsync(string email)
    {
         return this.Ok(await repository.ExistsByEmailAsync(email));
    }

    [HttpGet("exists/id/{userId}")]
    public async Task<ActionResult<bool>> ExistsByIdAsync(string userId)
    {
       return this.Ok(await repository.ExistsByIdAsync(userId));
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetUsersAsync(int pageNum = 1, int pageSize = 10)
    {
        var users = await repository.GetUsersAsync(pageNum, pageSize);
        return this.Ok(users);
    }
}
