 using MobyLabWebProgramming.Core.DataTransferObjects;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

 public interface IReviewService
 {
     Task<ReviewDTO?> GetReview(Guid id, CancellationToken cancellationToken = default);
     Task<List<ReviewDTO>> GetReviews(Guid? movieId = null, Guid? userId = null, CancellationToken cancellationToken = default);
     Task<ReviewDTO> AddReview(Guid userId, ReviewDTO review, CancellationToken cancellationToken = default);
     Task<ReviewDTO> UpdateReview(Guid userId, ReviewDTO review, CancellationToken cancellationToken = default);
     Task DeleteReview(Guid userId, Guid id, CancellationToken cancellationToken = default);
 }