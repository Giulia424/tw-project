using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Implementation;

  public class GenreRepository : IGenreRepository
    {
        private readonly WebAppDatabaseContext _dbContext;

        public GenreRepository(WebAppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Set<Genre>().FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<ICollection<Genre>> GetAllAsync(Expression<Func<Genre, bool>>? filter = null, int? skip = null, int? take = null,
            ICollection<Expression<Func<Genre, object>>>? includes = null, CancellationToken ct = default)
        {
            var query = _dbContext.Set<Genre>().AsQueryable();

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

        public async Task<int> GetCountAsync(Expression<Func<Genre, bool>>? filter = null, CancellationToken ct = default)
        {
            var query = _dbContext.Set<Genre>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync(ct);
        }

        public async Task<Genre> AddAsync(Genre entity, CancellationToken ct = default)
        {
            await _dbContext.Set<Genre>().AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<Genre> UpdateAsync(Genre entity, CancellationToken ct = default)
        {
            _dbContext.Set<Genre>().Update(entity);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            
            if (entity == null)
            {
                return false;
            }

            _dbContext.Set<Genre>().Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }
    }