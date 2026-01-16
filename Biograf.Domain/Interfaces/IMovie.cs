namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for movies.
/// </summary>
public interface IMovie
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task<Movie> AddAsync(Movie movie);
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteAsync(int id);
}
