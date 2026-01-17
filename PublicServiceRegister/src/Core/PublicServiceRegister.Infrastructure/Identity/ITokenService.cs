namespace PublicServiceRegister.Infrastructure.Identity;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(ApplicationUser user);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
}

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);
