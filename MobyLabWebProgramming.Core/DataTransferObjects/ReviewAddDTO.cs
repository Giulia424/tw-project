namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for creating a new review
/// </summary>
public class ReviewAddDTO
{
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }

    public string Content { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

}
