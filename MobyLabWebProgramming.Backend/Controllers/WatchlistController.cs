using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Security.Claims;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/watchlist")]
[Authorize]
public class WatchlistController : AuthorizedController
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService, IUserService userService) : base(userService)
    {
        _watchlistService = watchlistService;
    }

  

    [HttpGet("my-watchlist")]
    public async Task<ActionResult<List<WatchlistItemDTO>>> GetUserWatchlist(CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        var items = await _watchlistService.GetWatchlist(currentUser.Result.Id, cancellationToken);
        return Ok(items);
    }

    [HttpPost("watchlist-movie/{movieId}")]
    public async Task<ActionResult<WatchlistItemDTO>> AddMovieToWatchlist(Guid movieId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        var result = await _watchlistService.AddToWatchlist(currentUser.Result.Id, new WatchlistItemDTO { MovieId = movieId }, cancellationToken);
        return Ok(result);
    }


  
    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<WatchlistItemDTO>> GetMovieFromWatchlist(Guid movieId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        
        try
        {
            // Get the user's watchlist
            var userWatchlist = await _watchlistService.GetWatchlist(currentUser.Result.Id, cancellationToken);
            
            // Find the watchlist item with the specified movie ID
            var watchlistItem = userWatchlist.FirstOrDefault(w => w.MovieId == movieId);
            
            if (watchlistItem == null)
            {
                return NotFound(new { message = "Movie not found in your watchlist" });
            }
            
            return Ok(watchlistItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred", details = ex.Message });
        }
    }

    [HttpDelete("movie/{movieId}")]
    public async Task<ActionResult> DeleteMovieFromWatchlist(Guid movieId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        
        try
        {
            // Get the user's watchlist
            var userWatchlist = await _watchlistService.GetWatchlist(currentUser.Result.Id, cancellationToken);
            
            // Find the watchlist item with the specified movie ID
            var watchlistItem = userWatchlist.FirstOrDefault(w => w.MovieId == movieId);
            
            if (watchlistItem == null)
            {
                return NotFound(new { message = "Movie not found in your watchlist" });
            }
            
            // Delete the watchlist item
            await _watchlistService.DeleteFromWatchlist(currentUser.Result.Id, watchlistItem.Id, cancellationToken);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred", details = ex.Message });
        }
    }

    [HttpPut("movie/{movieId}")]
    public async Task<ActionResult<WatchlistItemDTO>> UpdateMovieInWatchlist(Guid movieId, [FromBody] bool watched, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        
        try
        {
            // Get the user's watchlist
            var userWatchlist = await _watchlistService.GetWatchlist(currentUser.Result.Id, cancellationToken);
            
            // Find the watchlist item with the specified movie ID
            var watchlistItem = userWatchlist.FirstOrDefault(w => w.MovieId == movieId);
            
            if (watchlistItem == null)
            {
                return NotFound(new { message = "Movie not found in your watchlist" });
            }
            
            // Update the watched status
            watchlistItem.Watched = watched;
            
            // Update the watchlist item
            var updatedItem = await _watchlistService.UpdateWatchlistItem(currentUser.Result.Id, watchlistItem, cancellationToken);
            
            return Ok(updatedItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred", details = ex.Message });
        }
    }
}