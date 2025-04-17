using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Implementation;

public class MovieRepository(WebAppDatabaseContext dbContext) : IMovieRepository
{
    public async Task<Movie?> GetMovieByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<List<Movie>> GetMoviesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Movies.ToListAsync(cancellationToken);

    public async Task<PagedResponse<Movie>> GetMoviesPageAsync(PaginationQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var totalCount = await dbContext.Movies.CountAsync(cancellationToken);
        var movies = await dbContext.Movies
            .OrderBy(m => m.Title)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<Movie>(pagination.Page, pagination.PageSize, totalCount, movies);
    }

    public async Task<Movie> AddMovieAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await dbContext.Movies.AddAsync(movie, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return movie;
    }

    public async Task<Movie> UpdateMovieAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        movie.UpdateTime();
        dbContext.Entry(movie).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
        return movie;
    }

    public async Task<int> DeleteMovieAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movie = await GetMovieByIdAsync(id, cancellationToken);
        if (movie == null) return 0;

        dbContext.Movies.Remove(movie);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Movie>> GetMoviesByGenreAsync(string genre, CancellationToken cancellationToken = default)
    {
        return await dbContext.Movies
            .Where(m => Object.Equals(genre, StringComparison.OrdinalIgnoreCase))
            .ToListAsync(cancellationToken);
    }

}
