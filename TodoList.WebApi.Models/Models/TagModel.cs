namespace TodoList.WebApi.Models.Models;

public class TagModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TodoListId { get; set; }
}
