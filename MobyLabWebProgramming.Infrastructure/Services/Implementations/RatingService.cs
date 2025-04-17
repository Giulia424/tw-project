using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class RatingService : IRatingService
{
    private readonly WebAppDatabaseContext _databaseContext;
    private readonly IMailService _mailService;

    public RatingService(WebAppDatabaseContext databaseContext, IMailService mailService)
    {
        _databaseContext = databaseContext;
        _mailService = mailService;
    }

    public async Task<RatingDTO?> GetRating(Guid id, CancellationToken cancellationToken = default)
    {
        var rating = await _databaseContext.Ratings
            .Include(r => r.User)
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (rating == null)
        {
            return null;
        }

        return new RatingDTO
        {
            Id = rating.Id,
            Value = rating.Value,
            UserId = rating.UserId,
            UserName = rating.User?.Name ?? "Unknown User",
            MovieId = rating.MovieId,
            MovieTitle = rating.Movie?.Title ?? "Unknown Movie",
            CreatedAt = rating.CreatedAt,
            UpdatedAt = rating.UpdatedAt
        };
    }

    public async Task<List<RatingDTO>> GetRatings(Guid? movieId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = _databaseContext.Ratings
            .Include(r => r.User)
            .Include(r => r.Movie)
            .AsQueryable();

        // Apply filters if provided
        if (movieId.HasValue)
        {
            query = query.Where(r => r.MovieId == movieId.Value);
        }

        if (userId.HasValue)
        {
            query = query.Where(r => r.UserId == userId.Value);
        }

        var ratings = await query.ToListAsync(cancellationToken);

        return ratings.Select(rating => new RatingDTO
        {
            Id = rating.Id,
            Value = rating.Value,
            UserId = rating.UserId,
            UserName = rating.User?.Name ?? "Unknown User",
            MovieId = rating.MovieId,
            MovieTitle = rating.Movie?.Title ?? "Unknown Movie",
            CreatedAt = rating.CreatedAt,
            UpdatedAt = rating.UpdatedAt
        }).ToList();
    }

    public async Task<RatingDTO> AddRating(Guid userId, RatingDTO rating, CancellationToken cancellationToken = default)
    {
        // Check if user has already rated this movie
        var existingRating = await _databaseContext.Ratings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == rating.MovieId, cancellationToken);

        if (existingRating != null)
        {
            throw new Exception("You have already rated this movie");
        }

        // Validate rating value
        if (rating.Value < 1 || rating.Value > 10)
        {
            throw new Exception("Rating value must be between 1 and 10");
        }

        // Create new rating
        var newRating = new Rating
        {
            UserId = userId,
            MovieId = rating.MovieId,
            Value = rating.Value
        };

        await _databaseContext.Ratings.AddAsync(newRating, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        // Get the movie title for notification
        var movie = await _databaseContext.Movies
            .FirstOrDefaultAsync(m => m.Id == rating.MovieId, cancellationToken);

        // Get the user's email for notification
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        // Send notification email if user and movie are found
        if (user != null && movie != null)
        {
            try
            {
                await _mailService.SendMail(
                    user.Email,
                    "Rating Added",
                    $"Your rating of {rating.Value} for '{movie.Title}' has been added successfully.",
                    true,
                    "MobyLab Movie Platform",
                    cancellationToken
                );
            }
            catch (Exception)
            {
                // Continue execution even if email fails
                // The rating was already added successfully
            }
        }

        // Return the created rating with additional information
        return new RatingDTO
        {
            Id = newRating.Id,
            Value = newRating.Value,
            UserId = newRating.UserId,
            UserName = user?.Name ?? "Unknown User",
            MovieId = newRating.MovieId,
            MovieTitle = movie?.Title ?? "Unknown Movie",
            CreatedAt = newRating.CreatedAt,
            UpdatedAt = newRating.UpdatedAt
        };
    }

    public async Task<RatingDTO> UpdateRating(Guid userId, RatingDTO rating, CancellationToken cancellationToken = default)
    {
        // Validate rating value
        if (rating.Value < 1 || rating.Value > 10)
        {
            throw new Exception("Rating value must be between 1 and 10");
        }

        // Find the existing rating
        var existingRating = await _databaseContext.Ratings
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == rating.Id && r.UserId == userId, cancellationToken);

        if (existingRating == null)
        {
            throw new Exception("Rating not found or you don't have permission to update it");
        }

        // Update the rating
        existingRating.Value = rating.Value;
        existingRating.UpdatedAt = DateTime.UtcNow;

        _databaseContext.Ratings.Update(existingRating);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        // Get the user's name for the response
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        // Return the updated rating
        return new RatingDTO
        {
            Id = existingRating.Id,
            Value = existingRating.Value,
            UserId = existingRating.UserId,
            UserName = user?.Name ?? "Unknown User",
            MovieId = existingRating.MovieId,
            MovieTitle = existingRating.Movie?.Title ?? "Unknown Movie",
            CreatedAt = existingRating.CreatedAt,
            UpdatedAt = existingRating.UpdatedAt
        };
    }

    public async Task DeleteRating(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        // Find the rating
        var rating = await _databaseContext.Ratings
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId, cancellationToken);

        if (rating == null)
        {
            throw new Exception("Rating not found or you don't have permission to delete it");
        }

        // Delete the rating
        _databaseContext.Ratings.Remove(rating);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
