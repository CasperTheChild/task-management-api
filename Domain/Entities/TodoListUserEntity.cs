using Domain.Enums;

namespace Domain.Entities;

public class TodoListUserEntity
{
    public string UserId { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;

    public TodoListRole Role { get; set; }
}
