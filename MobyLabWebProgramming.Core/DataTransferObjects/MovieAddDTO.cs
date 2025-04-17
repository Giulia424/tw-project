namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class MovieAddDTO
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Director { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public float Rating { get; set; }
    public List<Guid> GenreIds { get; set; } = new();
}