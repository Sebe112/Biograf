namespace Biograf.Application.Dtos.Auth;

/// <summary>
/// Login request payload.
/// </summary>
public class LoginRequest
{
    [Required]
    public string UsernameOrEmail { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
