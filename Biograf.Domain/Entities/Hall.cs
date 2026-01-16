namespace Biograf.Domain.Entities;

/// <summary>
/// Represents a cinema hall.
/// </summary>
public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public string Layout { get; set; } = "[]";
    public List<Seat> Seats { get; set; } = new();
    public List<Screening> Screenings { get; set; } = new();
}
