namespace Biograf.Api.Models;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<MovieGenre> Movies { get; set; } = new List<MovieGenre>();
}
