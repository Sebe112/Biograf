namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for movies.
/// </summary>
public class MovieRepository : IMovie
{
    private readonly BiografDbContext _context;

    public MovieRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        var query = from dbMovie in _context.Movies
                    select dbMovie;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        var query = from dbMovie in _context.Movies
                    where dbMovie.Id == id
                    select dbMovie;

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<Movie> AddAsync(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        var existsQuery = from dbMovie in _context.Movies
                          where dbMovie.Id == movie.Id
                          select dbMovie;

        var exists = await existsQuery.AnyAsync();
        if (!exists) return false;

        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = from dbMovie in _context.Movies
                    where dbMovie.Id == id
                    select dbMovie;

        var movie = await query.FirstOrDefaultAsync();
        if (movie == null) return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<Movie>> GetAllWithGenresAsync()
    {
        var query = _context.Movies
            .Include("MovieGenres")
            .Include("MovieGenres.Genre");

        var filtered = from dbMovie in query
                    select dbMovie;

        return await filtered.AsNoTracking().ToListAsync();
    }
}
