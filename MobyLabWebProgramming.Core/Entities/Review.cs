namespace MobyLabWebProgramming.Core.Entities;

public class Review : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    // Relationships
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}