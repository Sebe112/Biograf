namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a seat in a hall.
/// </summary>
public class Seat
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public Hall Hall { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public string Label { get; set; }
    public bool IsDisabledSeat { get; set; } = false;
}
