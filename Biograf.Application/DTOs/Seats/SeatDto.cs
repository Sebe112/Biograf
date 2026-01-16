namespace Biograf.Application.Dtos.Seats;

/// <summary>
/// Seat response payload.
/// </summary>
public class SeatDto
{
    public int Id { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public string Label { get; set; } = "";
    public bool IsDisabledSeat { get; set; }
}
