namespace Biograf.Application.Dtos.Movies;

/// <summary>
/// Movie response payload.
/// </summary>
public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
}
