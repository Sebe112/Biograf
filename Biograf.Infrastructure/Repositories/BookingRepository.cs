namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for bookings.
/// </summary>
public class BookingRepository : IBooking
{
    private readonly BiografDbContext _context;

    public BookingRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        var query = _context.Bookings
            .Include("BookingSeats");

        var filtered = from dbBooking in query
                       where dbBooking.Id == id
                       select dbBooking;

        return await filtered.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<List<Booking>> GetByUserIdAsync(string userId)
    {
        var query = _context.Bookings
            .Include("BookingSeats");

        var filtered = from dbBooking in query
                    where dbBooking.UserId == userId
                    orderby dbBooking.CreatedAt descending
                    select dbBooking;

        return await filtered.AsNoTracking().ToListAsync();
    }

    public async Task<Booking> AddAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> UpdateStatusAsync(int bookingId, BookingStatus status)
    {
        var query = from dbBooking in _context.Bookings
                    where dbBooking.Id == bookingId
                    select dbBooking;

        var booking = await query.FirstOrDefaultAsync();
        if (booking == null) return false;

        booking.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
}
