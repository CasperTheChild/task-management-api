using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CommentCreateModel
{
    [Required]
    [Length(1, 150)]
    public string Content { get; set; } = string.Empty;
}
