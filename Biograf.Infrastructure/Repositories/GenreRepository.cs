namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for genres.
/// </summary>
public class GenreRepository : IGenre
{
    private readonly BiografDbContext _context;

    public GenreRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        var query = from dbGenre in _context.Genres
                    orderby dbGenre.Name
                    select dbGenre;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        var query = from dbGenre in _context.Genres
                    where dbGenre.Id == id
                    select dbGenre;

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<Genre> AddAsync(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = from dbGenre in _context.Genres
                    where dbGenre.Id == id
                    select dbGenre;

        var genre = await query.FirstOrDefaultAsync();
        if (genre == null) return false;

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return true;
    }
}
