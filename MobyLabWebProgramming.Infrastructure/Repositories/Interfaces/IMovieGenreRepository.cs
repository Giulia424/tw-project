using System.Linq.Expressions;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

  public interface IMovieGenreRepository
    {
   
        Task<MovieGenre?> GetByMovieAndGenreAsync(Guid movieId, Guid genreId, CancellationToken ct = default);

 
        Task<ICollection<MovieGenre>> GetAllAsync(Expression<Func<MovieGenre, bool>>? filter = null, int? skip = null, int? take = null, 
            ICollection<Expression<Func<MovieGenre, object>>>? includes = null, CancellationToken ct = default);

        Task<ICollection<Genre>> GetGenresByMovieIdAsync(Guid movieId, CancellationToken ct = default);

        Task<ICollection<Movie>> GetMoviesByGenreIdAsync(Guid genreId, int? skip = null, int? take = null, CancellationToken ct = default);

       
        Task<int> GetCountAsync(Expression<Func<MovieGenre, bool>>? filter = null, CancellationToken ct = default);

      
        Task<MovieGenre> AddAsync(MovieGenre entity, CancellationToken ct = default);

        Task<bool> AddGenresToMovieAsync(Guid movieId, ICollection<Guid> genreIds, CancellationToken ct = default);

        Task<bool> DeleteAsync(Guid movieId, Guid genreId, CancellationToken ct = default);

        Task<bool> RemoveAllGenresFromMovieAsync(Guid movieId, CancellationToken ct = default);
    }
    