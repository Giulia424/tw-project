using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly WebAppDatabaseContext _databaseContext;
    private readonly IMailService _mailService;

    public ReviewService(WebAppDatabaseContext databaseContext, IMailService mailService)
    {
        _databaseContext = databaseContext;
        _mailService = mailService;
    }

    public async Task<ReviewDTO?> GetReview(Guid id, CancellationToken cancellationToken = default)
    {
        var review = await _databaseContext.Reviews
            .Include(r => r.User)
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (review == null)
        {
            return null;
        }

        return new ReviewDTO
        {
            Id = review.Id,
            Title = review.Title,
            Content = review.Content,
            UserId = review.UserId,
            UserName = review.User?.Name ?? "Unknown User",
            MovieId = review.MovieId,
            MovieTitle = review.Movie?.Title ?? "Unknown Movie",
        };
    }

    public async Task<List<ReviewDTO>> GetReviews(Guid? movieId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = _databaseContext.Reviews
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

        var reviews = await query.ToListAsync(cancellationToken);

        return reviews.Select(review => new ReviewDTO
        {
            Id = review.Id,
            Title = review.Title,
            Content = review.Content,
            UserId = review.UserId,
            UserName = review.User?.Name ?? "Unknown User",
            MovieId = review.MovieId,
            MovieTitle = review.Movie?.Title ?? "Unknown Movie",
        }).ToList();
    }

    public async Task<ReviewDTO> AddReview(Guid userId, ReviewDTO review, CancellationToken cancellationToken = default)
    {
        // Check if user has already reviewed this movie
        var existingReview = await _databaseContext.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == review.MovieId, cancellationToken);

        if (existingReview != null)
        {
            throw new Exception("You have already reviewed this movie");
        }

        // Create new review
        var newReview = new Review
        {
            UserId = userId,
            MovieId = review.MovieId,
            Title = review.Title,
            Content = review.Content
        };

        await _databaseContext.Reviews.AddAsync(newReview, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        // Get the movie title for notification
        var movie = await _databaseContext.Movies
            .FirstOrDefaultAsync(m => m.Id == review.MovieId, cancellationToken);

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
                    "Review Added",
                    $"Your review for '{movie.Title}' has been added successfully.",
                    true,
                    "MobyLab Movie Platform",
                    cancellationToken
                );
            }
            catch (Exception)
            {
                // Continue execution even if email fails
                // The review was already added successfully
            }
        }

        // Return the created review with additional information
        return new ReviewDTO
        {
            Id = newReview.Id,
            Title = newReview.Title,
            Content = newReview.Content,
            UserId = newReview.UserId,
            UserName = user?.Name ?? "Unknown User",
            MovieId = newReview.MovieId,
            MovieTitle = movie?.Title ?? "Unknown Movie",
        };
    }

    public async Task<ReviewDTO> UpdateReview(Guid userId, ReviewDTO review, CancellationToken cancellationToken = default)
    {
        // Find the existing review
        var existingReview = await _databaseContext.Reviews
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == review.Id && r.UserId == userId, cancellationToken);

        if (existingReview == null)
        {
            throw new Exception("Review not found or you don't have permission to update it");
        }

        // Update the review
        existingReview.Title = review.Title;
        existingReview.Content = review.Content;
        existingReview.UpdatedAt = DateTime.UtcNow;

        _databaseContext.Reviews.Update(existingReview);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        // Get the user's name for the response
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        // Return the updated review
        return new ReviewDTO
        {
            Id = existingReview.Id,
            Title = existingReview.Title,
            Content = existingReview.Content,
            UserId = existingReview.UserId,
            UserName = user?.Name ?? "Unknown User",
            MovieId = existingReview.MovieId,
            MovieTitle = existingReview.Movie?.Title ?? "Unknown Movie",
        };
    }

    public async Task DeleteReview(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        // Find the review
        var review = await _databaseContext.Reviews
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId, cancellationToken);

        if (review == null)
        {
            throw new Exception("Review not found or you don't have permission to delete it");
        }

        // Delete the review
        _databaseContext.Reviews.Remove(review);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
