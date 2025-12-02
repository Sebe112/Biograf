namespace Biograf.Api.Models;

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int DurationMinutes { get; set; }
    public required string AgeRating { get; set; }
    public string? PosterUrl { get; set; }

    public ICollection<MovieGenre> Genres { get; set; } = new List<MovieGenre>();
    public ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
