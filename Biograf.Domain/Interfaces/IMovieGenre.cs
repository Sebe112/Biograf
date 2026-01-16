namespace Biograf.Domain.Interfaces;

/// <summary>
/// Data access for movie-genre relations.
/// </summary>
public interface IMovieGenre
{
    Task<bool> AddGenreToMovieAsync(int movieId, int genreId);
    Task<bool> RemoveGenreFromMovieAsync(int movieId, int genreId);
}
