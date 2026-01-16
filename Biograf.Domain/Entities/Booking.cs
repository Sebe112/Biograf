namespace Biograf.Domain.Entities;

/// <summary>
/// Represents the booking state of an order.
/// </summary>
public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2
}

/// <summary>
/// Represents a booking for a screening.
/// </summary>
public class Booking
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int ScreeningId { get; set; }
    public Screening Screening { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public decimal TotalPrice { get; set; }
    public List<BookingSeat> BookingSeats { get; set; } = new();
}
