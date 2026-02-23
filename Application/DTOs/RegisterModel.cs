using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class RegisterModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
