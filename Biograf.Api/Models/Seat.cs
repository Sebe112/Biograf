namespace Biograf.Api.Models;

public class Seat
{
    public int Id { get; set; }
    public required string RowLabel { get; set; }
    public int Number { get; set; }
    public bool IsAisle { get; set; }

    public int HallId { get; set; }
    public Hall? Hall { get; set; }

    public ICollection<BookingSeat> Bookings { get; set; } = new List<BookingSeat>();

    public string Label => $"{RowLabel}{Number}";
}
