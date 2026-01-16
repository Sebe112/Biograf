namespace Biograf.Application.Dtos.Bookings;

/// <summary>
/// Seat line item for a booking.
/// </summary>
public class BookingSeatDto
{
    public int BookingId { get; set; }
    public int SeatId { get; set; }
    public int ScreeningId { get; set; }
    public decimal Price { get; set; }
}
