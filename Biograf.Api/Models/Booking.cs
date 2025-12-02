namespace Biograf.Api.Models;

public class Booking
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ScreeningId { get; set; }
    public Screening? Screening { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public string? CustomerEmail { get; set; }

    public ICollection<BookingSeat> Seats { get; set; } = new List<BookingSeat>();
}
