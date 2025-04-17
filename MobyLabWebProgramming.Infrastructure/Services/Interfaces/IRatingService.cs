 using MobyLabWebProgramming.Core.DataTransferObjects;
 using MobyLabWebProgramming.Core.Requests;
 using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

 public interface IRatingService
 {
     Task<RatingDTO?> GetRating(Guid id, CancellationToken cancellationToken = default);
     Task<List<RatingDTO>> GetRatings(Guid? movieId = null, Guid? userId = null, CancellationToken cancellationToken = default);
     Task<RatingDTO> AddRating(Guid userId, RatingDTO rating, CancellationToken cancellationToken = default);
     Task<RatingDTO> UpdateRating(Guid userId, RatingDTO rating, CancellationToken cancellationToken = default);
     Task DeleteRating(Guid userId, Guid id, CancellationToken cancellationToken = default);
 }