namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for movie-genre relations.
/// </summary>
public class MovieGenreRepository : IMovieGenre
{
    private readonly BiografDbContext _context;

    public MovieGenreRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddGenreToMovieAsync(int movieId, int genreId)
    {
        var existsQuery = from movieGenre in _context.MovieGenres
                          where movieGenre.MovieId == movieId && movieGenre.GenreId == genreId
                          select movieGenre;

        var exists = await existsQuery.AnyAsync();

        if (exists) return false;

        _context.MovieGenres.Add(new MovieGenre
        {
            MovieId = movieId,
            GenreId = genreId
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveGenreFromMovieAsync(int movieId, int genreId)
    {
        var query = from movieGenre in _context.MovieGenres
                    where movieGenre.MovieId == movieId && movieGenre.GenreId == genreId
                    select movieGenre;

        var mg = await query.FirstOrDefaultAsync();

        if (mg == null) return false;

        _context.MovieGenres.Remove(mg);
        await _context.SaveChangesAsync();
        return true;
    }
}
