namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for halls.
/// </summary>
public interface IHall
{
    Task<List<Hall>> GetAllAsync();
    Task<Hall?> GetByIdAsync(int id);
    Task<Hall> AddAsync(Hall hall);
    Task<bool> UpdateAsync(Hall hall);
    Task<bool> DeleteAsync(int id);
}
