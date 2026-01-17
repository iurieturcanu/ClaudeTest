using System.Net.Http.Json;
using PublicServiceRegister.Contracts;

namespace PublicServiceRegister.Officer.Web.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateService _authStateService;

    public AuthService(HttpClient httpClient, AuthenticationStateService authStateService)
    {
        _httpClient = httpClient;
        _authStateService = authStateService;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginRequest(email, password));
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    // Check if user has Admin role
                    if (!loginResponse.User.Roles.Contains("Admin"))
                    {
                        return false;
                    }
                    await _authStateService.LoginAsync(loginResponse);
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        var refreshToken = await _authStateService.GetRefreshTokenAsync();
        if (!string.IsNullOrEmpty(refreshToken))
        {
            try
            {
                await _httpClient.PostAsJsonAsync("api/auth/logout", new RefreshTokenRequest(refreshToken));
            }
            catch
            {
                // Ignore logout errors
            }
        }
        await _authStateService.LogoutAsync();
    }
}
