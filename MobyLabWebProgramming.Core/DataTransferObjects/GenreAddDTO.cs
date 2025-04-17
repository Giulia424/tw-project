namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for creating a new genre
/// </summary>
public class GenreAddDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}