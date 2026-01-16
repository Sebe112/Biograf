namespace Biograf.Application.Dtos.Bookings;

/// <summary>
/// Booking response with seats.
/// </summary>
public class BookingDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int ScreeningId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Status { get; set; }
    public decimal TotalPrice { get; set; }
    public List<BookingSeatDto> BookingSeats { get; set; } = new();
}
