using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicServiceRegister.Infrastructure.Identity;

namespace PublicServiceRegister.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
        {
            return Unauthorized(new { error = "Invalid email or password" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                return Unauthorized(new { error = "Account is locked. Please try again later." });
            }
            return Unauthorized(new { error = "Invalid email or password" });
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await _tokenService.GenerateTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new LoginResponse(
            token.AccessToken,
            token.RefreshToken,
            token.ExpiresAt,
            new UserInfo(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList())));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BadRequest(new { error = "Email is already registered" });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, "Public");

        var token = await _tokenService.GenerateTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new LoginResponse(
            token.AccessToken,
            token.RefreshToken,
            token.ExpiresAt,
            new UserInfo(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList())));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var token = await _tokenService.RefreshTokenAsync(request.RefreshToken);
        if (token == null)
        {
            return Unauthorized(new { error = "Invalid or expired refresh token" });
        }

        return Ok(new TokenResponse(token.AccessToken, token.RefreshToken, token.ExpiresAt));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        await _tokenService.RevokeTokenAsync(request.RefreshToken);
        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new UserInfo(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList()));
    }
}

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password, string FirstName, string LastName);
public record RefreshTokenRequest(string RefreshToken);
public record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
public record LoginResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserInfo User);
public record UserInfo(Guid Id, string Email, string FirstName, string LastName, List<string> Roles);
