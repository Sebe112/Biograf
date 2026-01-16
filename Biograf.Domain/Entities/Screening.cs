namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a scheduled screening of a movie.
/// </summary>
public class Screening
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int HallId { get; set; }
    public Hall Hall { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public decimal BasePrice { get; set; } = 0;
    public List<Booking> Bookings { get; set; } = new();
}
