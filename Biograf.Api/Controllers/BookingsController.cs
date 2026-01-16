using Biograf.Application.Dtos.Bookings;

namespace Biograf.Api.Controllers;

/// <summary>
/// Manages bookings for the authenticated user.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBooking _booking;
    private readonly IBookingSeat _bookingSeat;
    private readonly IScreening _screening;

    public BookingsController(IBooking booking, IBookingSeat bookingSeat, IScreening screening)
    {
        _booking = booking;
        _bookingSeat = bookingSeat;
        _screening = screening;
    }

    /// <summary>
    /// Creates a booking for the current user and selected seats.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request is required.");
        }

        if (request.ScreeningId <= 0)
        {
            return BadRequest("ScreeningId is required.");
        }

        if (request.SeatIds == null || request.SeatIds.Count == 0)
        {
            return BadRequest("SeatIds is required.");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var screening = await _screening.GetWithHallAndSeatsAsync(request.ScreeningId);
        if (screening == null)
        {
            return NotFound("Screening not found.");
        }

        if (screening.Hall == null || screening.Hall.Seats == null || screening.Hall.Seats.Count == 0)
        {
            return BadRequest("No seats found for this screening.");
        }

        var uniqueSeatIds = new List<int>();
        var seenSeatIds = new HashSet<int>();
        var invalidSeatIds = new List<int>();

        foreach (var seatId in request.SeatIds)
        {
            if (seatId <= 0)
            {
                invalidSeatIds.Add(seatId);
                continue;
            }

            if (seenSeatIds.Add(seatId))
            {
                uniqueSeatIds.Add(seatId);
            }
            else
            {
                invalidSeatIds.Add(seatId);
            }
        }

        if (invalidSeatIds.Count > 0)
        {
            return BadRequest(new { message = "Invalid or duplicate seat ids.", seatIds = invalidSeatIds });
        }

        var hallSeats = new Dictionary<int, Seat>();
        foreach (var seat in screening.Hall.Seats)
        {
            hallSeats[seat.Id] = seat;
        }

        var notInHall = new List<int>();
        var disabledSeatIds = new List<int>();

        foreach (var seatId in uniqueSeatIds)
        {
            if (!hallSeats.TryGetValue(seatId, out var seat))
            {
                notInHall.Add(seatId);
                continue;
            }

            if (seat.IsDisabledSeat)
            {
                disabledSeatIds.Add(seatId);
            }
        }

        if (notInHall.Count > 0)
        {
            return BadRequest(new { message = "Seat is not in the screening hall.", seatIds = notInHall });
        }

        if (disabledSeatIds.Count > 0)
        {
            return BadRequest(new { message = "Seat is disabled.", seatIds = disabledSeatIds });
        }

        var alreadyBooked = new List<int>();
        foreach (var seatId in uniqueSeatIds)
        {
            var isBooked = await _bookingSeat.IsSeatBookedAsync(request.ScreeningId, seatId);
            if (isBooked)
            {
                alreadyBooked.Add(seatId);
            }
        }

        if (alreadyBooked.Count > 0)
        {
            return Conflict(new { message = "Seat already booked.", seatIds = alreadyBooked });
        }

        var booking = new Booking();
        booking.UserId = userId;
        booking.ScreeningId = screening.Id;
        booking.Status = BookingStatus.Confirmed;
        booking.TotalPrice = screening.BasePrice * uniqueSeatIds.Count;

        booking = await _booking.AddAsync(booking);

        var seatPrice = screening.BasePrice;
        foreach (var seatId in uniqueSeatIds)
        {
            await _bookingSeat.AddAsync(booking.Id, seatId, screening.Id, seatPrice);
        }

        var dto = new BookingDto();
        dto.Id = booking.Id;
        dto.UserId = booking.UserId ?? "";
        dto.ScreeningId = booking.ScreeningId;
        dto.CreatedAt = booking.CreatedAt;
        dto.Status = (int)booking.Status;
        dto.TotalPrice = booking.TotalPrice;

        foreach (var seatId in uniqueSeatIds)
        {
            var seatDto = new BookingSeatDto();
            seatDto.BookingId = booking.Id;
            seatDto.SeatId = seatId;
            seatDto.ScreeningId = screening.Id;
            seatDto.Price = seatPrice;
            dto.BookingSeats.Add(seatDto);
        }

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, dto);
    }

    /// <summary>
    /// Returns bookings for the current user.
    /// </summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var bookings = await _booking.GetByUserIdAsync(userId);
        var result = new List<BookingDto>();

        foreach (var booking in bookings)
        {
            result.Add(MapBooking(booking));
        }

        return Ok(result);
    }

    /// <summary>
    /// Returns a booking by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _booking.GetByIdAsync(id);
        if (booking == null) return NotFound();
        return Ok(MapBooking(booking));
    }

    private static BookingDto MapBooking(Booking booking)
    {
        var dto = new BookingDto();
        dto.Id = booking.Id;
        dto.UserId = booking.UserId ?? "";
        dto.ScreeningId = booking.ScreeningId;
        dto.CreatedAt = booking.CreatedAt;
        dto.Status = (int)booking.Status;
        dto.TotalPrice = booking.TotalPrice;

        if (booking.BookingSeats != null)
        {
            foreach (var seat in booking.BookingSeats)
            {
                var seatDto = new BookingSeatDto();
                seatDto.BookingId = seat.BookingId;
                seatDto.SeatId = seat.SeatId;
                seatDto.ScreeningId = seat.ScreeningId;
                seatDto.Price = seat.Price;
                dto.BookingSeats.Add(seatDto);
            }
        }

        return dto;
    }
}
