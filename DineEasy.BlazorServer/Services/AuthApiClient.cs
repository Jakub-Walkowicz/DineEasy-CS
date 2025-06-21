using DineEasy.BlazorServer.Services;
using DineEasy.SharedKernel.Models.Auth;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthStateService _authStateService;
    
    public AuthApiClient(HttpClient httpClient, AuthStateService authStateService)
    {
        _httpClient = httpClient;
        _authStateService = authStateService;
    }
    
    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginDto);
        
        if (response.IsSuccessStatusCode)
        {
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            
            // Automatycznie ustaw stan autoryzacji
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                await _authStateService.SetAuthenticatedAsync(authResponse.Username, authResponse.Token);
            }
            
            return authResponse;
        }
        return null;
    }
    
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerDto);
        
        if (response.IsSuccessStatusCode)
        {
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                await _authStateService.SetAuthenticatedAsync(authResponse.Username, authResponse.Token);
            }
            
            return authResponse;
        }
        
        return null;
    }
}