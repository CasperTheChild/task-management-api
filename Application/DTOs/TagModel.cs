namespace Application.DTOs;

public class TagModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TodoListId { get; set; }
}
