using Biograf.Application.Dtos.Halls;

namespace Biograf.Application.Dtos.Screenings;

/// <summary>
/// Screening response payload with hall details.
/// </summary>
public class ScreeningWithHallDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int HallId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public decimal BasePrice { get; set; }
    public HallDto? Hall { get; set; }
}
