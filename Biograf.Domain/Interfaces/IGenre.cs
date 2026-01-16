namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for genres.
/// </summary>
public interface IGenre
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre> AddAsync(Genre genre);
    Task<bool> DeleteAsync(int id);
}
