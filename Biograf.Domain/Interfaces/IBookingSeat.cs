namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for booking seats.
/// </summary>
public interface IBookingSeat
{
    Task<List<BookingSeat>> GetByScreeningAsync(int screeningId);
    Task<bool> IsSeatBookedAsync(int screeningId, int seatId);
    Task AddAsync(int bookingId, int seatId, int screeningId, decimal price);
    Task<bool> RemoveAsync(int bookingId, int seatId);
}
