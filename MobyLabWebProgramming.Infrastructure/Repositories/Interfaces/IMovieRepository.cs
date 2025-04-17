using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<Movie?> GetMovieByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Movie>> GetMoviesAsync(CancellationToken cancellationToken = default);
    Task<PagedResponse<Movie>> GetMoviesPageAsync(PaginationQueryParams pagination, CancellationToken cancellationToken = default);
    Task<Movie> AddMovieAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<Movie> UpdateMovieAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<int> DeleteMovieAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Movie>> GetMoviesByGenreAsync(string genre, CancellationToken cancellationToken = default);
}