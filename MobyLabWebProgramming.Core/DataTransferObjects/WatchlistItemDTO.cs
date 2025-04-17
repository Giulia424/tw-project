using System;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class WatchlistItemDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MovieId { get; set; }
    public string? MovieName { get; set; }
    public DateTime AddedAt { get; set; }
    public bool Watched { get; set; }
}