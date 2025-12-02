namespace Biograf.Api.Models;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool IsAdmin { get; set; }
    public string? PasswordHash { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
