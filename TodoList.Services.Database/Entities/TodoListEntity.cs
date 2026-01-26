using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.Services.Database.Identity;

namespace TodoList.Services.Database.Entities;

public class TodoListEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.Today;

    public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

    [NotMapped]
    public bool HasOverdueTasks => this.Tasks != null && this.Tasks.Any(t => !t.IsCompleted && t.EndDate < DateTime.Now);

    public IEnumerable<TodoListUserEntity> Members { get; set; } = new List<TodoListUserEntity>();
}
