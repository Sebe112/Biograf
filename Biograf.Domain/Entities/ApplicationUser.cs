namespace Biograf.Domain.Entities;

/// <summary>
/// Application user with additional profile data.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
