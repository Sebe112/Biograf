namespace Biograf.Api.Controllers;

/// <summary>
/// Manages relations between movies and genres.
/// </summary>
[ApiController]
[Route("api/movie-genres")]
[Authorize(Roles = "Admin")]
public class MovieGenresController : ControllerBase
{
    private readonly IMovieGenre _movieGenre;

    public MovieGenresController(IMovieGenre movieGenre)
    {
        _movieGenre = movieGenre;
    }

    /// <summary>
    /// Adds a genre to a movie.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Add(int movieId, int genreId)
    {
        var ok = await _movieGenre.AddGenreToMovieAsync(movieId, genreId);
        if (!ok) return BadRequest("Relation already exists");
        return Ok();
    }

    /// <summary>
    /// Removes a genre from a movie.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Remove(int movieId, int genreId)
    {
        var ok = await _movieGenre.RemoveGenreFromMovieAsync(movieId, genreId);
        if (!ok) return NotFound();
        return NoContent();
    }
}
