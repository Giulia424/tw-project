namespace MobyLabWebProgramming.Core.Entities;

public class Rating : BaseEntity
{
    public int Value { get; set; } // 1-10 scale
    
    // Relationships
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}