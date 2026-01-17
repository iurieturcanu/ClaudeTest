using Blazored.LocalStorage;
using PublicServiceRegister.Contracts;

namespace PublicServiceRegister.Admin.Web.Services;

public class AuthenticationStateService
{
    private const string TokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";
    private const string UserKey = "currentUser";

    private readonly ILocalStorageService _localStorage;

    public event Action? OnAuthenticationStateChanged;

    public AuthenticationStateService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(TokenKey);
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        return await _localStorage.GetItemAsync<UserInfo>(UserKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task LoginAsync(LoginResponse response)
    {
        await _localStorage.SetItemAsStringAsync(TokenKey, response.AccessToken);
        await _localStorage.SetItemAsStringAsync(RefreshTokenKey, response.RefreshToken);
        await _localStorage.SetItemAsync(UserKey, response.User);
        OnAuthenticationStateChanged?.Invoke();
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
        OnAuthenticationStateChanged?.Invoke();
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(RefreshTokenKey);
    }
}
