namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a movie genre.
/// </summary>
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieGenre> MovieGenres { get; set; } = new();
}
