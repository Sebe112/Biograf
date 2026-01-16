namespace Biograf.Application.Dtos.Screenings;

/// <summary>
/// Request payload for creating a screening.
/// </summary>
public class ScreeningCreateRequest
{
    public int MovieId { get; set; }
    public int HallId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public decimal BasePrice { get; set; }
}
