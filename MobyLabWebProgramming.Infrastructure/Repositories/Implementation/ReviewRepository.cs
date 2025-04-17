// ReviewRepository.cs
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
    public class ReviewRepository : IReviewRepository
    {
        private readonly WebAppDatabaseContext _dbContext;

        public ReviewRepository(WebAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Review> GetByIdAsync(Guid id)
        {
            return await _dbContext.Reviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _dbContext.Reviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .ToListAsync();
        }

        public async Task<List<Review>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Reviews
                .Include(r => r.Movie)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Review>> GetByMovieIdAsync(Guid movieId)
        {
            return await _dbContext.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<Review> GetByUserAndMovieAsync(Guid userId, Guid movieId)
        {
            return await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _dbContext.Reviews.Update(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(Guid id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review != null)
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Reviews.AnyAsync(r => r.Id == id);
        }
    }
}