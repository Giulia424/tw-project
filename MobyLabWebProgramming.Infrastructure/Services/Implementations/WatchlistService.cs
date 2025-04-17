using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Repositories;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class WatchlistService : IWatchlistService
{
    private readonly IWatchlistItemRepository _watchlistRepository;
    private readonly WebAppDatabaseContext _databaseContext;

    public WatchlistService(IWatchlistItemRepository watchlistRepository, WebAppDatabaseContext databaseContext)
    {
        _watchlistRepository = watchlistRepository;
        _databaseContext = databaseContext;
    }

    public async Task<WatchlistItemDTO?> GetWatchlistItem(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _watchlistRepository.GetByIdAsync(id);
        if (item == null)
        {
            return null;
        }

        return new WatchlistItemDTO
        {
            Id = item.Id,
            UserId = item.UserId,
            MovieId = item.MovieId,
            MovieName = item.Movie.Title,
            AddedAt = item.CreatedAt,
            Watched = item.Watched
        };
    }

    public async Task<List<WatchlistItemDTO>> GetWatchlist(Guid userId, CancellationToken cancellationToken = default)
    {
        var items = await _watchlistRepository.GetByUserIdAsync(userId);
        return items.Select(item => new WatchlistItemDTO
        {
            Id = item.Id,
            UserId = item.UserId,
            MovieId = item.MovieId,
            MovieName = item.Movie.Title,
            AddedAt = item.CreatedAt,
            Watched = item.Watched
        }).ToList();
    }

    public async Task<WatchlistItemDTO> AddToWatchlist(Guid userId, WatchlistItemDTO item, CancellationToken cancellationToken = default)
    {
        var watchlistItem = new WatchlistItem
        {
            UserId = userId,
            MovieId = item.MovieId,
            Watched = false
        };

        var result = await _watchlistRepository.AddAsync(watchlistItem);
        
        return new WatchlistItemDTO
        {
            Id = result.Id,
            UserId = result.UserId,
            MovieId = result.MovieId,
            AddedAt = result.CreatedAt,
            Watched = result.Watched
        };
    }

    public async Task<WatchlistItemDTO> UpdateWatchlistItem(Guid userId, WatchlistItemDTO item, CancellationToken cancellationToken = default)
    {
        var existingItem = await _watchlistRepository.GetByIdAsync(item.Id);
        if (existingItem == null || existingItem.UserId != userId)
        {
            throw new Exception("Watchlist item not found");
        }

        existingItem.Watched = item.Watched;
        var result = await _watchlistRepository.UpdateAsync(existingItem);
        
        return new WatchlistItemDTO
        {
            Id = result.Id,
            UserId = result.UserId,
            MovieId = result.MovieId,
            AddedAt = result.CreatedAt,
            Watched = result.Watched
        };
    }

    public async Task DeleteFromWatchlist(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _watchlistRepository.GetByIdAsync(id);
        if (item != null && item.UserId == userId)
        {
            await _watchlistRepository.DeleteAsync(id);
        }
    }
}
