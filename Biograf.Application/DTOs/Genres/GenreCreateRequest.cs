namespace Biograf.Application.Dtos.Genres;

/// <summary>
/// Request payload for creating a genre.
/// </summary>
public class GenreCreateRequest
{
    public string Name { get; set; } = "";
}
