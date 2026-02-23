namespace Application.DTOs;

public class PaginatedModel<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int TotalItems { get; set; }

    public int ItemsPerPage { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages => ItemsPerPage > 0 ? (int)Math.Ceiling((float)TotalItems / ItemsPerPage): 0;
}
