namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for bookings.
/// </summary>
public interface IBooking
{
    Task<Booking?> GetByIdAsync(int id);
    Task<List<Booking>> GetByUserIdAsync(string userId);
    Task<Booking> AddAsync(Booking booking);
    Task<bool> UpdateStatusAsync(int bookingId, BookingStatus status);
}
