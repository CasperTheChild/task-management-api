namespace Application.DTOs;

public class SearchParameterModel
{
    public string? Text { get; set; }

    public SearchFilterModel Filter { get; set; } = new SearchFilterModel();

    public SearchSortModel Sort { get; set; } = new SearchSortModel();
}
