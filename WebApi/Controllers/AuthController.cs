using Application.DTOs;
using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository service;

        public AuthController(IAuthRepository service)
        {
            this.service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            bool success = await service.RegisterAsync(model.Email, model.Password);
            if (!success)
            {
                return BadRequest("Registration failed.");
            }

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            string? token = await service.LoginAsync(model.Email, model.Password);
            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new { Token = token });
        }
    }
}
