namespace MobyLabWebProgramming.Core.Entities;

using MobyLabWebProgramming.Core.Enums;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRoleEnum Role { get; set; }
    
    // Navigation properties
    public ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();
    
    // New navigation properties
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<WatchlistItem> WatchlistItems { get; set; } = new List<WatchlistItem>();
}