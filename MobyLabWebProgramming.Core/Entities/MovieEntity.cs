namespace MobyLabWebProgramming.Core.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Genre { get; set; }
    public int Rating { get; set; }
    public string Director { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    
    // Navigation properties
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<WatchlistItem> WatchlistItems { get; set; } = new List<WatchlistItem>();
}