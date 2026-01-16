namespace Biograf.Application.Services;

/// <summary>
/// Creates JWT tokens for authenticated users.
/// </summary>
public class JwtTokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    /// <summary>
    /// Builds a signed JWT token for a user and returns auth details.
    /// </summary>
    public async Task<AuthResponse> CreateTokenAsync(ApplicationUser user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new Exception("Jwt:Key missing");
        var issuer = jwt["Issuer"] ?? throw new Exception("Jwt:Issuer missing");
        var audience = jwt["Audience"] ?? throw new Exception("Jwt:Audience missing");
        var expiresMinutes = int.TryParse(jwt["ExpiresMinutes"], out var m) ? m : 120;

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? "")
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAt,
            UserId = user.Id,
            Username = user.UserName ?? "",
            Roles = roles
        };
    }
}
