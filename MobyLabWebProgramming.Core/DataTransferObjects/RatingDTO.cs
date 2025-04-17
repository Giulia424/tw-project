namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class RatingDTO
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public int Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

   
}