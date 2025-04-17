namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for updating an existing watchlist item
/// </summary>
public class WatchlistItemUpdateDTO
{
    public Guid Id { get; set; }
    public bool IsWatched { get; set; }
}
