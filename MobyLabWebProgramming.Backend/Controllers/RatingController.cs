using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class RatingController : AuthorizedController
{
    private readonly IRatingService _ratingService;
    private readonly IMovieService _movieService;

    public RatingController(IRatingService ratingService, IUserService userService, IMovieService movieService) : base(userService)
    {
        _ratingService = ratingService;
        _movieService = movieService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RatingDTO>> GetRating([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        var rating = await _ratingService.GetRating(id, cancellationToken);
        
        if (rating == null)
        {
            return NotFound();
        }
        
        return Ok(rating);
    }

    [HttpGet]
    public async Task<ActionResult<List<RatingDTO>>> GetRatings(
        [FromQuery] Guid? movieId = null, 
        [FromQuery] Guid? userId = null, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        var ratings = await _ratingService.GetRatings(movieId, userId, cancellationToken);
        return Ok(ratings);
    }

    [HttpPost("movie/{movieId:guid}")]
    public async Task<ActionResult<RatingDTO>> AddRating(
        [FromRoute] Guid movieId,
        [FromBody] RatingAddDTO rating, 
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
        
        var ratingDto = new RatingDTO
        {
            MovieId = movieId,
            UserId = currentUser.Result.Id,
            Value = rating.Score
        };
        
        try
        {
            var result = await _ratingService.AddRating(currentUser.Result.Id, ratingDto, cancellationToken);
            return CreatedAtAction(nameof(GetRating), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RatingDTO>> UpdateRating(
        [FromRoute] Guid id,
        [FromBody] RatingUpdateDTO rating, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }

        // Get the existing rating to verify ownership
        var existingRating = await _ratingService.GetRating(id, cancellationToken);
        if (existingRating == null)
        {
            return NotFound(new { message = "Rating not found" });
        }

        if (existingRating.UserId != currentUser.Result.Id)
        {
            return Forbid();
        }
        
        var ratingDto = new RatingDTO
        {
            Id = id,
            Value = rating.Score
        };
        
        try
        {
            var result = await _ratingService.UpdateRating(currentUser.Result.Id, ratingDto, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteRating(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        
        try
        {
            await _ratingService.DeleteRating(currentUser.Result.Id, id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}