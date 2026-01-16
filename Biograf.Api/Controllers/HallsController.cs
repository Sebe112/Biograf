using Biograf.Application.Dtos.Halls;

namespace Biograf.Api.Controllers;

/// <summary>
/// Manages cinema halls.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallsController : ControllerBase
{
    private readonly IHall _hall;

    public HallsController(IHall hall)
    {
        _hall = hall;
    }

    /// <summary>
    /// Returns all halls.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var halls = await _hall.GetAllAsync();
        var result = new List<HallDto>();

        foreach (var hall in halls)
        {
            result.Add(MapHall(hall));
        }

        return Ok(result);
    }

    /// <summary>
    /// Returns a hall by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var hall = await _hall.GetByIdAsync(id);
        if (hall == null) return NotFound();
        return Ok(MapHall(hall));
    }

    /// <summary>
    /// Creates a new hall.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(HallCreateRequest request)
    {
        if (request.Rows <= 0 || request.Columns <= 0)
        {
            return BadRequest("Rows and columns must be greater than 0.");
        }

        var hall = new Hall();
        hall.Name = request.Name;
        hall.Rows = request.Rows;
        hall.Columns = request.Columns;

        var created = await _hall.AddAsync(hall);
        return Ok(MapHall(created));
    }

    /// <summary>
    /// Updates an existing hall.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, HallCreateRequest request)
    {
        if (request.Rows <= 0 || request.Columns <= 0)
        {
            return BadRequest("Rows and columns must be greater than 0.");
        }

        var hall = new Hall();
        hall.Id = id;
        hall.Name = request.Name;
        hall.Rows = request.Rows;
        hall.Columns = request.Columns;
        hall.Layout = "[]";

        var ok = await _hall.UpdateAsync(hall);
        if (!ok) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Deletes a hall by id.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _hall.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    private static HallDto MapHall(Hall hall)
    {
        var dto = new HallDto();
        dto.Id = hall.Id;
        dto.Name = hall.Name;
        dto.Rows = hall.Rows;
        dto.Columns = hall.Columns;
        return dto;
    }
}
