using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PublicServiceRegister.Officer.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateService _authStateService;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiService(HttpClient httpClient, AuthenticationStateService authStateService)
    {
        _httpClient = httpClient;
        _authStateService = authStateService;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
    }

    public async Task PostAsync<TRequest>(string endpoint, TRequest data)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    public async Task PostAsync(string endpoint)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsync(endpoint, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync<TRequest>(string endpoint, TRequest data)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string endpoint)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }

    private async Task SetAuthHeaderAsync()
    {
        var token = await _authStateService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
