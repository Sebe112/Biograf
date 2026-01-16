namespace Biograf.Application.Dtos.Bookings;

/// <summary>
/// Request payload for creating a booking.
/// </summary>
public class BookingCreateRequest
{
    public int ScreeningId { get; set; }
    public List<int> SeatIds { get; set; } = new();
}
