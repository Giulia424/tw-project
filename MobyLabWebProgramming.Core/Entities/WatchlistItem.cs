namespace MobyLabWebProgramming.Core.Entities;

public class WatchlistItem : BaseEntity
{
    public bool Watched { get; set; }
    
    // Relationships
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}