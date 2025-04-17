// IWatchlistItemRepository.cs
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Repositories
{
    public interface IWatchlistItemRepository
    {
        Task<WatchlistItem> GetByIdAsync(Guid id);
        Task<List<WatchlistItem>> GetAllAsync();
        Task<List<WatchlistItem>> GetByUserIdAsync(Guid userId);
        Task<WatchlistItem> GetByUserAndMovieAsync(Guid userId, Guid movieId);
        Task<WatchlistItem> AddAsync(WatchlistItem watchlistItem);
        Task<WatchlistItem> UpdateAsync(WatchlistItem watchlistItem);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}