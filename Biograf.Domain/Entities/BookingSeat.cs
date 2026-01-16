namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a seat reserved as part of a booking.
/// </summary>
public class BookingSeat
{
    public int BookingId { get; set; }
    public Booking Booking { get; set; }
    public int SeatId { get; set; }
    public Seat Seat { get; set; }
    public int ScreeningId { get; set; }
    public decimal Price { get; set; }
}
