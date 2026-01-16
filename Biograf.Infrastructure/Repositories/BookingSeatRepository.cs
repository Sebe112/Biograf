namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for booking seats.
/// </summary>
public class BookingSeatRepository : IBookingSeat
{
    private readonly BiografDbContext _context;

    public BookingSeatRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookingSeat>> GetByScreeningAsync(int screeningId)
    {
        var query = from dbBookingSeat in _context.BookingSeats
                    where dbBookingSeat.ScreeningId == screeningId
                    select dbBookingSeat;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<bool> IsSeatBookedAsync(int screeningId, int seatId)
    {
        var query = from dbBookingSeat in _context.BookingSeats
                    where dbBookingSeat.ScreeningId == screeningId && dbBookingSeat.SeatId == seatId
                    select dbBookingSeat;

        return await query.AsNoTracking().AnyAsync();
    }

    public async Task AddAsync(int bookingId, int seatId, int screeningId, decimal price)
    {
        _context.BookingSeats.Add(new BookingSeat
        {
            BookingId = bookingId,
            SeatId = seatId,
            ScreeningId = screeningId,
            Price = price
        });

        await _context.SaveChangesAsync();
    }

    public async Task<bool> RemoveAsync(int bookingId, int seatId)
    {
        var query = from dbBookingSeat in _context.BookingSeats
                    where dbBookingSeat.BookingId == bookingId && dbBookingSeat.SeatId == seatId
                    select dbBookingSeat;

        var bookingSeat = await query.FirstOrDefaultAsync();

        if (bookingSeat == null) return false;

        _context.BookingSeats.Remove(bookingSeat);
        await _context.SaveChangesAsync();
        return true;
    }
}
