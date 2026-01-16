namespace Biograf.Application.Dtos.Auth;

/// <summary>
/// Registration request payload.
/// </summary>
public class RegisterRequest
{
    [Required, MinLength(3)]
    public string Username { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(6)]
    public string Password { get; set; } = default!;
}
