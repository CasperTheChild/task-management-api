using TodoList.Services.Database.Identity;
using TodoList.WebApi.Models.Enums;

namespace TodoList.Services.Database.Entities;

public class TodoListUserEntity
{
    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;

    public TodoListRole Role { get; set; }
}
