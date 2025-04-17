using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly IMovieService _movieService;

    public GenreController(IGenreService genreService, IMovieService movieService)
    {
        _genreService = genreService;
        _movieService = movieService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponse<GenreDTO>>> GetGenre([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _genreService.GetGenre(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GenreDTO>>>> GetAllGenres(CancellationToken cancellationToken = default)
    {
        var result = await _genreService.GetAllGenres(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/movies")]
    public async Task<ActionResult<ServiceResponse<List<MovieDTO>>>> GetMoviesByGenre([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _movieService.GetMoviesByGenre(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> AddGenre([FromBody] GenreAddDTO genre, CancellationToken cancellationToken = default)
    {
        var result = await _genreService.AddGenre(genre, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse>> UpdateGenre([FromBody] GenreUpdateDTO genre, CancellationToken cancellationToken = default)
    {
        var result = await _genreService.UpdateGenre(genre, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> DeleteGenre([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _genreService.DeleteGenre(id, cancellationToken);
        return Ok(result);
    }
}
