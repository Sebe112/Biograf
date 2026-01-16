using Biograf.Application.Dtos.Movies;

namespace Biograf.Api.Controllers;

/// <summary>
/// Manages movie catalog entries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovie _movie;

    public MoviesController(IMovie movie)
    {
        _movie = movie;
    }

    /// <summary>
    /// Returns all movies.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movie.GetAllAsync();
        var result = new List<MovieDto>();

        foreach (var movie in movies)
        {
            result.Add(MapMovie(movie));
        }

        return Ok(result);
    }

    /// <summary>
    /// Returns a movie by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _movie.GetByIdAsync(id);
        if (movie == null) return NotFound();
        return Ok(MapMovie(movie));
    }

    /// <summary>
    /// Creates a new movie.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(MovieCreateRequest request)
    {
        var movie = new Movie();
        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.DurationMinutes = request.DurationMinutes;

        var created = await _movie.AddAsync(movie);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapMovie(created));
    }

    /// <summary>
    /// Updates an existing movie.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MovieCreateRequest request)
    {
        var movie = new Movie();
        movie.Id = id;
        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.DurationMinutes = request.DurationMinutes;

        var ok = await _movie.UpdateAsync(movie);
        if (!ok) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Deletes a movie by id.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _movie.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    private static MovieDto MapMovie(Movie movie)
    {
        var dto = new MovieDto();
        dto.Id = movie.Id;
        dto.Title = movie.Title;
        dto.Description = movie.Description;
        dto.DurationMinutes = movie.DurationMinutes;
        return dto;
    }
}
