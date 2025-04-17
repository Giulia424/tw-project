using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ReviewController : AuthorizedController
{
    private readonly IReviewService _reviewService;
    private readonly IMovieService _movieService;

    public ReviewController(IReviewService reviewService, IUserService userService, IMovieService movieService) : base(userService)
    {
        _reviewService = reviewService;
        _movieService = movieService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReviewDTO>> GetReview([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        var review = await _reviewService.GetReview(id, cancellationToken);
        
        if (review == null)
        {
            return NotFound();
        }
        
        return Ok(review);
    }

    [HttpGet]
    public async Task<ActionResult<List<ReviewDTO>>> GetReviews(
        [FromQuery] Guid? movieId = null, 
        [FromQuery] Guid? userId = null, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        var reviews = await _reviewService.GetReviews(movieId, userId, cancellationToken);
        return Ok(reviews);
    }

    [HttpPost("movie/{movieId:guid}")]
    public async Task<ActionResult<ReviewDTO>> AddReview(
        [FromRoute] Guid movieId,
        [FromBody] ReviewAddDTO review, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        // Get movie details
        var movie = await _movieService.GetMovie(movieId, cancellationToken);
        if (movie.Result == null)
        {
            return NotFound(new { message = "Movie not found" });
        }
        
        var reviewDto = new ReviewDTO
        {
            MovieId = movieId,
            UserId = currentUser.Result.Id,
            Title = review.Title,
            Content = review.Content,
            MovieTitle = movie.Result.Title,
            UserName = currentUser.Result.Name
        };
        
        var result = await _reviewService.AddReview(currentUser.Result.Id, reviewDto, cancellationToken);
        return CreatedAtAction(nameof(GetReview), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ReviewDTO>> UpdateReview(
        [FromRoute] Guid id,
        [FromBody] ReviewUpdateDTO review, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        // Get the existing review to get movie information
        var existingReview = await _reviewService.GetReview(id, cancellationToken);
        if (existingReview == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        if (existingReview.UserId != currentUser.Result.Id)
        {
            return Forbid();
        }
        
        var reviewDto = new ReviewDTO
        {
            Id = id,
            MovieId = existingReview.MovieId,
            UserId = currentUser.Result.Id,
            Title = review.Title,
            Content = review.Content,
            MovieTitle = existingReview.MovieTitle,
            UserName = currentUser.Result.Name
        };
        
        var result = await _reviewService.UpdateReview(currentUser.Result.Id, reviewDto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteReview(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        
        await _reviewService.DeleteReview(currentUser.Result.Id, id, cancellationToken);
        return NoContent();
    }
}
