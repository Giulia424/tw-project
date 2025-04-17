// IReviewRepository.cs
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Repositories
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(Guid id);
        Task<List<Review>> GetAllAsync();
        Task<List<Review>> GetByUserIdAsync(Guid userId);
        Task<List<Review>> GetByMovieIdAsync(Guid movieId);
        Task<Review> GetByUserAndMovieAsync(Guid userId, Guid movieId);
        Task<Review> AddAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}