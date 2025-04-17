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

    [HttpGet("watchlist-movie/{id}")]
    public async Task<ActionResult<WatchlistItemDTO>> GetWatchlistItemById(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        var item = await _watchlistService.GetWatchlistItem(id, cancellationToken);
        return item != null ? Ok(item) : NotFound();
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

    [HttpPut("watchlist-movie")]
    public async Task<ActionResult<WatchlistItemDTO>> UpdateWatchlistItem([FromBody] WatchlistItemDTO item, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        // Create a new item with only the necessary fields
        var updateItem = new WatchlistItemDTO
        {
            Id = item.Id,
            Watched = item.Watched
        };
        var result = await _watchlistService.UpdateWatchlistItem(currentUser.Result.Id, updateItem, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("watchlist-movie/{id}")]
    public async Task<ActionResult> RemoveFromWatchlist(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return Unauthorized(new { message = "User not authenticated", details = currentUser.Error });
        }
        await _watchlistService.DeleteFromWatchlist(currentUser.Result.Id, id, cancellationToken);
        return NoContent();
    }
}