namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for creating a new rating
/// </summary>
public class RatingAddDTO
{
    public Guid MovieId { get; set; }
    public int Score { get; set; }
}
