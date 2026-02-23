using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TagCreateModel
{
    [MaxLength(15)]
    public string Name { get; set; } = string.Empty;
}
