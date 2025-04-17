// IRatingRepository.cs
using MobyLabWebProgramming.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MobyLabWebProgramming.Core.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating> GetByIdAsync(Guid id);
        Task<List<Rating>> GetAllAsync();
        Task<List<Rating>> GetByUserIdAsync(Guid userId);
        Task<List<Rating>> GetByMovieIdAsync(Guid movieId);
        Task<Rating> GetByUserAndMovieAsync(Guid userId, Guid movieId);
        Task<Rating> AddAsync(Rating rating);
        Task<Rating> UpdateAsync(Rating rating);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}