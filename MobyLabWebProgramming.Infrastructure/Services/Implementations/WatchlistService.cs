using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Repositories;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Constants;
using System.Net;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class WatchlistService : IWatchlistService
{
    private readonly IWatchlistItemRepository _watchlistRepository;
    private readonly WebAppDatabaseContext _databaseContext;
    private readonly IMailService _mailService;

    public WatchlistService(IWatchlistItemRepository watchlistRepository, WebAppDatabaseContext databaseContext, IMailService mailService)
    {
        _watchlistRepository = watchlistRepository;
        _databaseContext = databaseContext;
        _mailService = mailService;
    }

    public async Task<WatchlistItemDTO> GetWatchlistItem(Guid id, CancellationToken cancellationToken = default)
    {
        // First, try to get the watchlist item directly
        var item = await _databaseContext.WatchlistItems
            .Include(w => w.Movie)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
            
        if (item == null)
        {
            // If not found, try to get it from the database using a different approach
            var watchlistItems = await _databaseContext.WatchlistItems
                .Include(w => w.Movie)
                .ToListAsync(cancellationToken);
                
            item = watchlistItems.FirstOrDefault(w => w.Id == id);
            
            if (item == null)
            {
                throw new Exception("Watchlist item not found");
            }
        }

        return new WatchlistItemDTO
        {
            Id = item.Id,
            UserId = item.UserId,
            MovieId = item.MovieId,
            MovieName = item.Movie?.Title ?? "Unknown Movie",
            AddedAt = item.CreatedAt,
            Watched = item.Watched
        };
    }

    public async Task<List<WatchlistItemDTO>> GetWatchlist(Guid userId, CancellationToken cancellationToken = default)
    {
        var items = await _databaseContext.WatchlistItems
            .Include(w => w.Movie)
            .Where(w => w.UserId == userId)
            .ToListAsync(cancellationToken);
        
        return items.Select(item => new WatchlistItemDTO
        {
            Id = item.Id,
            UserId = item.UserId,
            MovieId = item.MovieId,
            MovieName = item.Movie?.Title ?? "Unknown Movie",
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
        
        // Get the user's email and the movie title
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            
        var movie = await _databaseContext.Movies
            .FirstOrDefaultAsync(m => m.Id == item.MovieId, cancellationToken);
        
        if (user != null && movie != null)
        {
            try
            {
                await _mailService.SendMail(
                    user.Email,
                    "Movie Added to Watchlist",
                    EmailTemplates.MovieAddedToWatchlistNotification(movie.Title),
                    true,
                    "MobyLab Movie Platform",
                    cancellationToken
                );
            }
            catch (Exception)
            {
                // Continue execution even if email fails
                // The watchlist item was already added successfully
            }
        }
        
        return new WatchlistItemDTO
        {
            Id = result.Id,
            UserId = result.UserId,
            MovieId = result.MovieId,
            MovieName = movie?.Title ?? "Unknown Movie",
            AddedAt = result.CreatedAt,
            Watched = result.Watched
        };
    }

    public async Task<WatchlistItemDTO> UpdateWatchlistItem(Guid userId, WatchlistItemDTO item, CancellationToken cancellationToken = default)
    {
        var existingItem = await _databaseContext.WatchlistItems
            .Include(w => w.Movie)
            .FirstOrDefaultAsync(w => w.Id == item.Id && w.UserId == userId, cancellationToken);
            
        if (existingItem == null)
        {
            throw new Exception("Watchlist item not found");
        }

        existingItem.Watched = item.Watched;
        _databaseContext.WatchlistItems.Update(existingItem);
        await _databaseContext.SaveChangesAsync(cancellationToken);
        
        return new WatchlistItemDTO
        {
            Id = existingItem.Id,
            UserId = existingItem.UserId,
            MovieId = existingItem.MovieId,
            MovieName = existingItem.Movie?.Title ?? "Unknown Movie",
            AddedAt = existingItem.CreatedAt,
            Watched = existingItem.Watched
        };
    }

    public async Task DeleteFromWatchlist(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _databaseContext.WatchlistItems
            .Include(w => w.Movie)
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, cancellationToken);
            
        if (item == null)
        {
            throw new Exception("Watchlist item not found");
        }
        
        _databaseContext.WatchlistItems.Remove(item);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
