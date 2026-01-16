using Biograf.Application.Dtos.Bookings;

namespace Biograf.Api.Controllers;

/// <summary>
/// Provides access to booked seats for screenings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingSeatsController : ControllerBase
{
    private readonly IBookingSeat bookingSeatRepository;

    public BookingSeatsController(IBookingSeat bookingSeatRepository)
    {
        this.bookingSeatRepository = bookingSeatRepository;
    }

    /// <summary>
    /// Returns booked seats for a screening.
    /// </summary>
    [HttpGet("by-screening/{screeningId:int}")]
    public async Task<IActionResult> GetByScreening(int screeningId)
    {
        var seats = await bookingSeatRepository.GetByScreeningAsync(screeningId);
        var result = new List<BookingSeatDto>();

        foreach (var seat in seats)
        {
            var dto = new BookingSeatDto();
            dto.BookingId = seat.BookingId;
            dto.SeatId = seat.SeatId;
            dto.ScreeningId = seat.ScreeningId;
            dto.Price = seat.Price;
            result.Add(dto);
        }

        return Ok(result);
    }

    /// <summary>
    /// Removes a seat from a booking.
    /// </summary>
    [HttpDelete("booking/{bookingId:int}/seat/{seatId:int}")]
    public async Task<IActionResult> RemoveSeat(int bookingId, int seatId)
    {
        var success = await bookingSeatRepository.RemoveAsync(bookingId, seatId);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
