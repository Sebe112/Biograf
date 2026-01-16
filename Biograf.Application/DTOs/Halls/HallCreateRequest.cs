namespace Biograf.Application.Dtos.Halls;

/// <summary>
/// Request payload for creating or updating a hall.
/// </summary>
public class HallCreateRequest
{
    public string Name { get; set; } = "";
    public int Rows { get; set; }
    public int Columns { get; set; }
}
