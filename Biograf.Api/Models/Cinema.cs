namespace Biograf.Api.Models;

public class Cinema
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public int ScreenCount { get; set; }

    public ICollection<Hall> Halls { get; set; } = new List<Hall>();
}
