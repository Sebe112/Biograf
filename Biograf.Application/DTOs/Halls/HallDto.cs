namespace Biograf.Application.Dtos.Halls;

/// <summary>
/// Hall response payload.
/// </summary>
public class HallDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Rows { get; set; }
    public int Columns { get; set; }
}
