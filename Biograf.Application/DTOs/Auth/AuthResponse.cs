namespace Biograf.Application.Dtos.Auth;

/// <summary>
/// Authentication response containing token and user info.
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }
    public string UserId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public IList<string> Roles { get; set; } = new List<string>();
}
