 using MobyLabWebProgramming.Core.DataTransferObjects;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

 public interface IWatchlistService
 {
     Task<WatchlistItemDTO?> GetWatchlistItem(Guid id, CancellationToken cancellationToken = default);
     Task<List<WatchlistItemDTO>> GetWatchlist(Guid userId, CancellationToken cancellationToken = default);
     Task<WatchlistItemDTO> AddToWatchlist(Guid userId, WatchlistItemDTO item, CancellationToken cancellationToken = default);
     Task<WatchlistItemDTO> UpdateWatchlistItem(Guid userId, WatchlistItemDTO item, CancellationToken cancellationToken = default);
     Task DeleteFromWatchlist(Guid userId, Guid id, CancellationToken cancellationToken = default);
 } 