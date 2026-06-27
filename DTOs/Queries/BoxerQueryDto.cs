namespace FightClub.DTOs.Queries;

public class BoxerQueryDto
{
    public string? WeightCategory { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
}
