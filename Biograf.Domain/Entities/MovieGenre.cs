namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a movie-genre relation.
/// </summary>
public class MovieGenre
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}
