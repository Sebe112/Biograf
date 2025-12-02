namespace Biograf.Api.Models;

public class Hall
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int SeatCount { get; set; }
    public required string Sound { get; set; }
    public string? ScreenType { get; set; }

    public int CinemaId { get; set; }
    public Cinema? Cinema { get; set; }

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
