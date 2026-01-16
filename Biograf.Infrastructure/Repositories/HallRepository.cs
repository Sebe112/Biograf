namespace Biograf.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for halls.
/// </summary>
public class HallRepository : IHall
{
    private readonly BiografDbContext _context;

    public HallRepository(BiografDbContext context)
    {
        _context = context;
    }

    public async Task<List<Hall>> GetAllAsync()
    {
        var query = from dbHall in _context.Halls
                    select dbHall;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Hall?> GetByIdAsync(int id)
    {
        var query = from dbHall in _context.Halls
                    where dbHall.Id == id
                    select dbHall;

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<Hall> AddAsync(Hall hall)
    {
        _context.Halls.Add(hall);
        await _context.SaveChangesAsync();

        if (hall.Rows > 0 && hall.Columns > 0)
        {
            var seats = BuildSeats(hall.Id, hall.Rows, hall.Columns);
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
        }

        return hall;
    }

    public async Task<bool> UpdateAsync(Hall hall)
    {
        var query = _context.Halls
            .Include("Seats");

        var filtered = from dbHall in query
                       where dbHall.Id == hall.Id
                       select dbHall;

        var hallEntity = await filtered.FirstOrDefaultAsync();
        if (hallEntity == null) return false;

        var sizeChanged = hallEntity.Rows != hall.Rows || hallEntity.Columns != hall.Columns;

        hallEntity.Name = hall.Name;
        hallEntity.Rows = hall.Rows;
        hallEntity.Columns = hall.Columns;
        hallEntity.Layout = hall.Layout;

        if (sizeChanged)
        {
            _context.Seats.RemoveRange(hallEntity.Seats);
            var seats = BuildSeats(hallEntity.Id, hallEntity.Rows, hallEntity.Columns);
            _context.Seats.AddRange(seats);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = from dbHall in _context.Halls
                    where dbHall.Id == id
                    select dbHall;

        var hall = await query.FirstOrDefaultAsync();
        if (hall == null) return false;

        _context.Halls.Remove(hall);
        await _context.SaveChangesAsync();
        return true;
    }

    private static List<Seat> BuildSeats(int hallId, int rows, int columns)
    {
        var seats = new List<Seat>();

        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                var seat = new Seat();
                seat.HallId = hallId;
                seat.RowIndex = rowIndex;
                seat.ColumnIndex = columnIndex;
                seat.Label = BuildSeatLabel(rowIndex, columnIndex);
                seat.IsDisabledSeat = false;
                seats.Add(seat);
            }
        }

        return seats;
    }

    private static string BuildSeatLabel(int rowIndex, int columnIndex)
    {
        if (rowIndex >= 0 && rowIndex < 26)
        {
            var rowLetter = (char)('A' + rowIndex);
            return rowLetter + (columnIndex + 1).ToString();
        }

        return "R" + (rowIndex + 1) + "C" + (columnIndex + 1);
    }
}
