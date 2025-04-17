using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Implementation;

public class MovieGenreRepository : IMovieGenreRepository
    {
        private readonly WebAppDatabaseContext _dbContext;

        public MovieGenreRepository(WebAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MovieGenre?> GetByMovieAndGenreAsync(Guid movieId, Guid genreId, CancellationToken ct = default)
        {
            return await _dbContext.Set<MovieGenre>()
                .FirstOrDefaultAsync(mg => mg.MovieId == movieId && mg.GenreId == genreId, ct);
        }

        public async Task<ICollection<MovieGenre>> GetAllAsync(Expression<Func<MovieGenre, bool>>? filter = null, int? skip = null, int? take = null,
            ICollection<Expression<Func<MovieGenre, object>>>? includes = null, CancellationToken ct = default)
        {
            var query = _dbContext.Set<MovieGenre>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync(ct);
        }

        public async Task<ICollection<Genre>> GetGenresByMovieIdAsync(Guid movieId, CancellationToken ct = default)
        {
            return await _dbContext.Set<MovieGenre>()
                .Where(mg => mg.MovieId == movieId)
                .Select(mg => mg.Genre)
                .ToListAsync(ct);
        }

        public async Task<ICollection<Movie>> GetMoviesByGenreIdAsync(Guid genreId, int? skip = null, int? take = null, CancellationToken ct = default)
        {
            var query = _dbContext.Set<MovieGenre>()
                .Where(mg => mg.GenreId == genreId)
                .Select(mg => mg.Movie);

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync(ct);
        }

        public async Task<int> GetCountAsync(Expression<Func<MovieGenre, bool>>? filter = null, CancellationToken ct = default)
        {
            var query = _dbContext.Set<MovieGenre>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync(ct);
        }

        public async Task<MovieGenre> AddAsync(MovieGenre entity, CancellationToken ct = default)
        {
            // Check if relationship already exists
            var existing = await GetByMovieAndGenreAsync(entity.MovieId, entity.GenreId, ct);
            if (existing != null)
            {
                return existing;
            }

            await _dbContext.Set<MovieGenre>().AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> AddGenresToMovieAsync(Guid movieId, ICollection<Guid> genreIds, CancellationToken ct = default)
        {
            try
            {
                // Get existing genre relationships
                var existingRelationships = await _dbContext.Set<MovieGenre>()
                    .Where(mg => mg.MovieId == movieId)
                    .Select(mg => mg.GenreId)
                    .ToListAsync(ct);

                // Add new relationships
                foreach (var genreId in genreIds.Where(id => !existingRelationships.Contains(id)))
                {
                    await _dbContext.Set<MovieGenre>().AddAsync(new MovieGenre
                    {
                        MovieId = movieId,
                        GenreId = genreId
                    }, ct);
                }

                await _dbContext.SaveChangesAsync(ct);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid movieId, Guid genreId, CancellationToken ct = default)
        {
            var entity = await GetByMovieAndGenreAsync(movieId, genreId, ct);
            
            if (entity == null)
            {
                return false;
            }

            _dbContext.Set<MovieGenre>().Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> RemoveAllGenresFromMovieAsync(Guid movieId, CancellationToken ct = default)
        {
            var relationships = await _dbContext.Set<MovieGenre>()
                .Where(mg => mg.MovieId == movieId)
                .ToListAsync(ct);

            if (!relationships.Any())
            {
                return true;
            }

            _dbContext.Set<MovieGenre>().RemoveRange(relationships);
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }
    }