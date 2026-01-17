namespace PublicServiceRegister.Contracts;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password, string FirstName, string LastName);
public record RefreshTokenRequest(string RefreshToken);

public record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
public record LoginResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserInfo User);
public record UserInfo(Guid Id, string Email, string FirstName, string LastName, List<string> Roles);

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    List<string> Roles);

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    List<string> Roles);

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    bool IsActive,
    List<string> Roles);
