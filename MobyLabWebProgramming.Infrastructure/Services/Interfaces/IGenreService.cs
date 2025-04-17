using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IGenreService
{
    Task<ServiceResponse<GenreDTO>> GetGenre(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<List<GenreDTO>>> GetAllGenres(CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddGenre(GenreAddDTO genre, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateGenre(GenreUpdateDTO genre, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteGenre(Guid id, CancellationToken cancellationToken = default);
}