namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class MovieDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Director { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public float Rating { get; set; }
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    
}