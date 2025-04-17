namespace MobyLabWebProgramming.Core.Entities;
// MovieGenre (Many-to-Many Relationship)
/// <summary>
/// Entity for many-to-many) relationship between Movie and Genre
/// </summary>
public class MovieGenre : BaseEntity
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    
    public Guid GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}