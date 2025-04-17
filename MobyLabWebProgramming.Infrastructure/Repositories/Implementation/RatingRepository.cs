// RatingRepository.cs
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
    public class RatingRepository : IRatingRepository
    {
        private readonly WebAppDatabaseContext _dbContext;

        public RatingRepository(WebAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Rating> GetByIdAsync(Guid id)
        {
            return await _dbContext.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Rating>> GetAllAsync()
        {
            return await _dbContext.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .ToListAsync();
        }

        public async Task<List<Rating>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Ratings
                .Include(r => r.Movie)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Rating>> GetByMovieIdAsync(Guid movieId)
        {
            return await _dbContext.Ratings
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<Rating> GetByUserAndMovieAsync(Guid userId, Guid movieId)
        {
            return await _dbContext.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
        }

        public async Task<Rating> AddAsync(Rating rating)
        {
            await _dbContext.Ratings.AddAsync(rating);
            await _dbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating> UpdateAsync(Rating rating)
        {
            _dbContext.Ratings.Update(rating);
            await _dbContext.SaveChangesAsync();
            return rating;
        }

        public async Task DeleteAsync(Guid id)
        {
            var rating = await _dbContext.Ratings.FindAsync(id);
            if (rating != null)
            {
                _dbContext.Ratings.Remove(rating);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Ratings.AnyAsync(r => r.Id == id);
        }
    }
}