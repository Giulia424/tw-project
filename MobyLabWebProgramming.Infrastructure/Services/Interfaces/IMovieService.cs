using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IMovieService
    {
        Task<ServiceResponse<MovieDTO>> GetMovie(Guid id, CancellationToken cancellationToken = default);
        Task<ServiceResponse<List<MovieDTO>>> GetAllMovies(CancellationToken cancellationToken = default);
        Task<ServiceResponse<List<MovieDTO>>> GetMoviesByGenre(Guid genreId, CancellationToken cancellationToken = default);
        Task<ServiceResponse> AddMovie(MovieAddDTO movie, CancellationToken cancellationToken = default);
        Task<ServiceResponse> UpdateMovie(MovieUpdateDTO movie, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteMovie(Guid id, CancellationToken cancellationToken = default);
    }
}
                               