using Application.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        this.userManager = userManager;
        this.configuration = configuration;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
        };

        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<string?> LoginAsync(string login, string password)
    {
        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            return null;

            // Generate JWT token
            //var jwtSettings = configuration.GetSection("Jwt");
            //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var token = new JwtSecurityToken(
            //    issuer: jwtSettings["Issuer"],
            //    audience: jwtSettings["Audience"],
            //    claims: null,
            //    expires: DateTime.Now.AddMinutes(30),
            //    signingCredentials: creds);
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        return GenerateToken(user);
    }

    private string GenerateToken(ApplicationUser user)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim("sub", user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
