namespace TodoList.Services.Database.Entities;

public class TagEntity
{
    public int Id { get; set; }

    public string TagName { get; set; } = string.Empty;

    public string NormalizedTagName { get; set; } = string.Empty;

    public ICollection<TaskTagEntity> TaskTags { get; set; } = new List<TaskTagEntity>();
}
