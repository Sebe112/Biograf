namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for screenings.
/// </summary>
public class ScreeningRepository : IScreening
{
    private readonly BiografDbContext _context;

    public ScreeningRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<List<Screening>> GetAllAsync()
    {
        var query = from dbScreening in _context.Screenings
                    select dbScreening;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Screening?> GetByIdAsync(int id)
    {
        var query = from dbScreening in _context.Screenings
                    where dbScreening.Id == id
                    select dbScreening;

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<Screening?> GetWithHallAndSeatsAsync(int id)
    {
        var query = _context.Screenings
            .Include("Hall")
            .Include("Hall.Seats");

        var filtered = from dbScreening in query
                       where dbScreening.Id == id
                       select dbScreening;

        return await filtered.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<Screening> AddAsync(Screening screening)
    {
        _context.Screenings.Add(screening);
        await _context.SaveChangesAsync();
        return screening;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = from dbScreening in _context.Screenings
                    where dbScreening.Id == id
                    select dbScreening;

        var screening = await query.FirstOrDefaultAsync();
        if (screening == null) return false;

        _context.Screenings.Remove(screening);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(Screening screening)
    {
        var existsQuery = from dbScreening in _context.Screenings
                          where dbScreening.Id == screening.Id
                          select dbScreening;

        var exists = await existsQuery.AnyAsync();
        if (!exists) return false;

        _context.Screenings.Update(screening);
        await _context.SaveChangesAsync();
        return true;
    }
}
