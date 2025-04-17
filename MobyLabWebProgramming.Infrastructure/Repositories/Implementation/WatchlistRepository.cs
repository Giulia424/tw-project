// WatchlistItemRepository.cs
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Repositories;
using MobyLabWebProgramming.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Repositories
{
    public class WatchlistItemRepository : IWatchlistItemRepository
    {
        private readonly WebAppDatabaseContext _dbContext;

        public WatchlistItemRepository(WebAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WatchlistItem> GetByIdAsync(Guid id)
        {
            Console.WriteLine($"Repository: Attempting to get watchlist item with ID: {id}");
            
            var item = await _dbContext.WatchlistItems
                .Include(w => w.User)
                .Include(w => w.Movie)
                .FirstOrDefaultAsync(w => w.Id == id);
                
            if (item == null)
            {
                Console.WriteLine($"Repository: Watchlist item with ID {id} not found");
            }
            else
            {
                Console.WriteLine($"Repository: Found watchlist item with ID {id}, MovieId: {item.MovieId}, Movie Title: {item.Movie?.Title ?? "Unknown"}");
            }
            
            return item;
        }

        public async Task<List<WatchlistItem>> GetAllAsync()
        {
            return await _dbContext.WatchlistItems
                .Include(w => w.User)
                .Include(w => w.Movie)
                .ToListAsync();
        }

        public async Task<List<WatchlistItem>> GetByUserIdAsync(Guid userId)
        {
            Console.WriteLine($"Repository: Attempting to get watchlist items for user with ID: {userId}");
            
            var items = await _dbContext.WatchlistItems
                .Include(w => w.Movie)
                .Where(w => w.UserId == userId)
                .ToListAsync();
                
            Console.WriteLine($"Repository: Found {items.Count} watchlist items for user with ID: {userId}");
            
            return items;
        }

        public async Task<WatchlistItem> GetByUserAndMovieAsync(Guid userId, Guid movieId)
        {
            return await _dbContext.WatchlistItems
                .Include(w => w.Movie)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == movieId);
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem watchlistItem)
        {
            await _dbContext.WatchlistItems.AddAsync(watchlistItem);
            await _dbContext.SaveChangesAsync();
            return watchlistItem;
        }

        public async Task<WatchlistItem> UpdateAsync(WatchlistItem watchlistItem)
        {
            _dbContext.WatchlistItems.Update(watchlistItem);
            await _dbContext.SaveChangesAsync();
            return watchlistItem;
        }

        public async Task DeleteAsync(Guid id)
        {
            var watchlistItem = await _dbContext.WatchlistItems
                .Include(w => w.Movie)
                .FirstOrDefaultAsync(w => w.Id == id);
                
            if (watchlistItem != null)
            {
                _dbContext.WatchlistItems.Remove(watchlistItem);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.WatchlistItems.AnyAsync(w => w.Id == id);
        }
    }
}