using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieGenreRepository _movieGenreRepository;

    public MovieService(IMovieRepository movieRepository, IMovieGenreRepository movieGenreRepository)
    {
        _movieRepository = movieRepository;
        _movieGenreRepository = movieGenreRepository;
    }

    public async Task<ServiceResponse<MovieDTO>> GetMovie(Guid id, CancellationToken cancellationToken = default)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(id, cancellationToken);

        if (movie == null)
        {
            return ServiceResponse.FromError<MovieDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Movie not found", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse<MovieDTO>.ForSuccess(new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            Rating = movie.Rating
        });
    }

    public async Task<ServiceResponse<List<MovieDTO>>> GetAllMovies(CancellationToken cancellationToken = default)
    {
        var movies = await _movieRepository.GetMoviesAsync(cancellationToken);

        var movieDTOs = movies.Select(movie => new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            Rating = movie.Rating
        }).ToList();

        return ServiceResponse<List<MovieDTO>>.ForSuccess(movieDTOs);
    }

    public async Task<ServiceResponse<List<MovieDTO>>> GetMoviesByGenre(Guid genreId, CancellationToken cancellationToken = default)
    {
        var movies = await _movieGenreRepository.GetMoviesByGenreIdAsync(genreId, ct: cancellationToken);

        var movieDTOs = movies.Select(movie => new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            Rating = movie.Rating
        }).ToList();

        return ServiceResponse<List<MovieDTO>>.ForSuccess(movieDTOs);
    }

    public async Task<ServiceResponse> AddMovie(MovieAddDTO movie, CancellationToken cancellationToken = default)
    {
        var newMovie = new Movie
        {
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            Rating = (int)movie.Rating
        };

        await _movieRepository.AddMovieAsync(newMovie, cancellationToken);

        // Add genre relationships
        if (movie.GenreIds != null && movie.GenreIds.Any())
        {
            await _movieGenreRepository.AddGenresToMovieAsync(newMovie.Id, movie.GenreIds, cancellationToken);
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateMovie(MovieUpdateDTO movie, CancellationToken cancellationToken = default)
    {
        var existingMovie = await _movieRepository.GetMovieByIdAsync(movie.Id, cancellationToken);

        if (existingMovie == null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Movie not found", ErrorCodes.EntityNotFound));
        }

        existingMovie.Title = movie.Title ?? existingMovie.Title;
        existingMovie.Description = movie.Description ?? existingMovie.Description;
        existingMovie.ReleaseDate = movie.ReleaseDate ?? existingMovie.ReleaseDate;
        existingMovie.Rating = movie.Rating != null ? (int)movie.Rating : existingMovie.Rating;

        await _movieRepository.UpdateMovieAsync(existingMovie, cancellationToken);

        // Update genre relationships
        if (movie.GenreIds != null)
        {
            // First, remove all existing genre relationships
            await _movieGenreRepository.RemoveAllGenresFromMovieAsync(movie.Id, cancellationToken);
            
            // Then add the new genre relationships
            await _movieGenreRepository.AddGenresToMovieAsync(movie.Id, movie.GenreIds, cancellationToken);
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteMovie(Guid id, CancellationToken cancellationToken = default)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(id, cancellationToken);

        if (movie == null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Movie not found", ErrorCodes.EntityNotFound));
        }

        await _movieRepository.DeleteMovieAsync(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}
