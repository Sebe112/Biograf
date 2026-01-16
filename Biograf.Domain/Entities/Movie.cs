namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a movie in the catalog.
/// </summary>
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public List<Screening> Screenings { get; set; } = new();
    public List<MovieGenre> MovieGenres { get; set; } = new();
}
