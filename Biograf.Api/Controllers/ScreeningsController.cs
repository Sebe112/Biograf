using Biograf.Application.Dtos.Halls;
using Biograf.Application.Dtos.Seats;
using Biograf.Application.Dtos.Screenings;

namespace Biograf.Api.Controllers;

/// <summary>
/// Manages movie screenings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ScreeningsController : ControllerBase
{
    private readonly IScreening _screening;

    public ScreeningsController(IScreening screening)
    {
        _screening = screening;
    }

    /// <summary>
    /// Returns all screenings.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var screenings = await _screening.GetAllAsync();
        var result = new List<ScreeningDto>();

        foreach (var screening in screenings)
        {
            result.Add(MapScreening(screening));
        }

        return Ok(result);
    }

    /// <summary>
    /// Returns a screening by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var screening = await _screening.GetByIdAsync(id);
        if (screening == null) return NotFound();
        return Ok(MapScreening(screening));
    }

    /// <summary>
    /// Returns a screening with hall details.
    /// </summary>
    [HttpGet("{id:int}/with-hall")]
    public async Task<IActionResult> GetWithHall(int id)
    {
        var screening = await _screening.GetWithHallAndSeatsAsync(id);
        if (screening == null) return NotFound();
        return Ok(MapScreeningWithHall(screening));
    }

    /// <summary>
    /// Returns seats for the screening's hall.
    /// </summary>
    [HttpGet("{id:int}/seats")]
    public async Task<IActionResult> GetSeats(int id)
    {
        var screening = await _screening.GetWithHallAndSeatsAsync(id);
        if (screening == null) return NotFound();
        if (screening.Hall == null || screening.Hall.Seats == null)
        {
            return Ok(new List<SeatDto>());
        }

        var result = new List<SeatDto>();

        foreach (var seat in screening.Hall.Seats)
        {
            var dto = new SeatDto();
            dto.Id = seat.Id;
            dto.RowIndex = seat.RowIndex;
            dto.ColumnIndex = seat.ColumnIndex;
            dto.Label = seat.Label;
            dto.IsDisabledSeat = seat.IsDisabledSeat;
            result.Add(dto);
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates a new screening.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(ScreeningCreateRequest request)
    {
        var screening = new Screening();
        screening.MovieId = request.MovieId;
        screening.HallId = request.HallId;
        screening.StartsAt = request.StartsAt;
        screening.EndsAt = request.EndsAt;
        screening.BasePrice = request.BasePrice;

        var created = await _screening.AddAsync(screening);
        return Ok(MapScreening(created));
    }

    /// <summary>
    /// Deletes a screening by id.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _screening.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    private static ScreeningDto MapScreening(Screening screening)
    {
        var dto = new ScreeningDto();
        dto.Id = screening.Id;
        dto.MovieId = screening.MovieId;
        dto.HallId = screening.HallId;
        dto.StartsAt = screening.StartsAt;
        dto.EndsAt = screening.EndsAt;
        dto.BasePrice = screening.BasePrice;
        return dto;
    }

    private static ScreeningWithHallDto MapScreeningWithHall(Screening screening)
    {
        var dto = new ScreeningWithHallDto();
        dto.Id = screening.Id;
        dto.MovieId = screening.MovieId;
        dto.HallId = screening.HallId;
        dto.StartsAt = screening.StartsAt;
        dto.EndsAt = screening.EndsAt;
        dto.BasePrice = screening.BasePrice;

        if (screening.Hall != null)
        {
            var hallDto = new HallDto();
            hallDto.Id = screening.Hall.Id;
            hallDto.Name = screening.Hall.Name;
            hallDto.Rows = screening.Hall.Rows;
            hallDto.Columns = screening.Hall.Columns;
            dto.Hall = hallDto;
        }

        return dto;
    }
}
