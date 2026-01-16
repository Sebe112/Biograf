using Biograf.Application.Dtos.Genres;

namespace Biograf.Api.Controllers;

/// <summary>
/// Manages genre catalog entries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenre _genre;

    public GenresController(IGenre genre)
    {
        _genre = genre;
    }

    /// <summary>
    /// Returns all genres.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _genre.GetAllAsync();
        var result = new List<GenreDto>();

        foreach (var genre in genres)
        {
            var dto = new GenreDto();
            dto.Id = genre.Id;
            dto.Name = genre.Name;
            result.Add(dto);
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(GenreCreateRequest request)
    {
        var genre = new Genre();
        genre.Name = request.Name;

        var created = await _genre.AddAsync(genre);
        var dto = new GenreDto();
        dto.Id = created.Id;
        dto.Name = created.Name;
        return Ok(dto);
    }

    /// <summary>
    /// Deletes a genre by id.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _genre.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
