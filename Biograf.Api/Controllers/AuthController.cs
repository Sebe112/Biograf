namespace Biograf.Api.Controllers;

/// <summary>
/// Handles registration and login for users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtTokenService _jwt;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtTokenService jwt)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwt = jwt;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var user = new ApplicationUser
        {
            UserName = req.Username,
            Email = req.Email
        };

        var result = await _userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "User");

        var token = await _jwt.CreateTokenAsync(user);
        return Ok(token);
    }

    /// <summary>
    /// Logs in a user and returns a JWT token.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _userManager.FindByNameAsync(req.UsernameOrEmail)
                   ?? await _userManager.FindByEmailAsync(req.UsernameOrEmail);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var ok = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);
        if (!ok.Succeeded)
            return Unauthorized("Invalid credentials");

        var token = await _jwt.CreateTokenAsync(user);
        return Ok(token);
    }
}
