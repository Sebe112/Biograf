namespace Biograf.Application.Dtos.Movies;

/// <summary>
/// Request payload for creating or updating a movie.
/// </summary>
public class MovieCreateRequest
{
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
}
