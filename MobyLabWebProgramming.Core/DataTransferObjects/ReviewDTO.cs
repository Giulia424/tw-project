namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ReviewDTO
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public required string Content { get; set; }
    public required string Title { get; set; }
    public required string MovieTitle { get; set; }
    public required string UserName { get; set; }



}