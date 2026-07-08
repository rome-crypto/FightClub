namespace FightClub.DTOs.Common;

public abstract class BaseQueryDto
{
    public int Page { get; set; } = 1;
    private const int MaxPageSize = 100;
    private int _pageSize = 10;
    public int PageSize
    { 
        get => _pageSize; 
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}
