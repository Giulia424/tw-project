namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class MovieUpdateDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public float? Rating { get; set; }
    public List<Guid>? GenreIds { get; set; }
}