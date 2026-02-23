using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories;

public class MyCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public MyCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => this.httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
        ?? this.httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
