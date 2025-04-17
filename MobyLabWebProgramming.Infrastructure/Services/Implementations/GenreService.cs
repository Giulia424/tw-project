using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<ServiceResponse<GenreDTO>> GetGenre(Guid id, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken);

        if (genre == null)
        {
            return ServiceResponse.FromError<GenreDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Genre not found", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse<GenreDTO>.ForSuccess(new GenreDTO
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        });
    }

    public async Task<ServiceResponse<List<GenreDTO>>> GetAllGenres(CancellationToken cancellationToken = default)
    {
        var genres = await _genreRepository.GetAllAsync(ct: cancellationToken);

        var genreDTOs = genres.Select(genre => new GenreDTO
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        }).ToList();

        return ServiceResponse<List<GenreDTO>>.ForSuccess(genreDTOs);
    }

    public async Task<ServiceResponse> AddGenre(GenreAddDTO genre, CancellationToken cancellationToken = default)
    {
        var newGenre = new Genre
        {
            Name = genre.Name,
            Description = genre.Description
        };

        await _genreRepository.AddAsync(newGenre, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateGenre(GenreUpdateDTO genre, CancellationToken cancellationToken = default)
    {
        var existingGenre = await _genreRepository.GetByIdAsync(genre.Id, cancellationToken);

        if (existingGenre == null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Genre not found", ErrorCodes.EntityNotFound));
        }

        existingGenre.Name = genre.Name ?? existingGenre.Name;
        existingGenre.Description = genre.Description ?? existingGenre.Description;

        await _genreRepository.UpdateAsync(existingGenre, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteGenre(Guid id, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken);

        if (genre == null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Genre not found", ErrorCodes.EntityNotFound));
        }

        await _genreRepository.DeleteAsync(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}
