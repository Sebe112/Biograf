namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for seats.
/// </summary>
public interface ISeat
{
    Task<List<Seat>> GetByHallIdAsync(int hallId);
    Task<Seat?> GetByIdAsync(int id);
    Task<Seat> AddAsync(Seat seat);
    Task<bool> DeleteAsync(int id);
}
