using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponse<MovieDTO>>> GetMovie([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _movieService.GetMovie(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<MovieDTO>>>> GetAllMovies(CancellationToken cancellationToken = default)
    {
        var result = await _movieService.GetAllMovies(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> AddMovie([FromBody] MovieAddDTO movie, CancellationToken cancellationToken = default)
    {
        var result = await _movieService.AddMovie(movie, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse>> UpdateMovie([FromBody] MovieUpdateDTO movie, CancellationToken cancellationToken = default)
    {
        var result = await _movieService.UpdateMovie(movie, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> DeleteMovie([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _movieService.DeleteMovie(id, cancellationToken);
        return Ok(result);
    }
}
