namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for seats.
/// </summary>
public class SeatRepository : ISeat
{
    private readonly BiografDbContext _context;

    public SeatRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<List<Seat>> GetByHallIdAsync(int hallId)
    {
        var query = from dbSeat in _context.Seats
                    where dbSeat.HallId == hallId
                    orderby dbSeat.RowIndex, dbSeat.ColumnIndex
                    select dbSeat;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Seat?> GetByIdAsync(int id)
    {
        var query = from dbSeat in _context.Seats
                    where dbSeat.Id == id
                    select dbSeat;

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<Seat> AddAsync(Seat seat)
    {
        _context.Seats.Add(seat);
        await _context.SaveChangesAsync();
        return seat;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = from dbSeat in _context.Seats
                    where dbSeat.Id == id
                    select dbSeat;

        var seat = await query.FirstOrDefaultAsync();
        if (seat == null) return false;

        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync();
        return true;
    }
}
