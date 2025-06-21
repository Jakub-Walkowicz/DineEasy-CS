using Microsoft.JSInterop;

namespace DineEasy.BlazorServer.Services;

public class AuthStateService
{
    private readonly IJSRuntime _jsRuntime;
    private string? _token;
    
    public bool IsAuthenticated { get; private set; }
    public string? Username { get; private set; }
    public string? Token => _token;
    
    public event Action? OnChange;

    public AuthStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetAuthenticatedAsync(string username, string token)
    {
        IsAuthenticated = true;
        Username = username;
        _token = token;
        
        // Zapisz token w localStorage
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "username", username);
        
        OnChange?.Invoke();
    }

    public async Task SetUnauthenticatedAsync()
    {
        IsAuthenticated = false;
        Username = null;
        _token = null;
        
        // Usuń token z localStorage
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "username");
        
        OnChange?.Invoke();
    }

    public async Task LogoutAsync()
    {
        await SetUnauthenticatedAsync();
    }

    // Metoda do odtworzenia stanu po odświeżeniu strony
    public async Task InitializeAsync()
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
            var username = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "username");

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(username))
            {
                IsAuthenticated = true;
                Username = username;
                _token = token;

                // Dodaj to:
                OnChange?.Invoke();
            }
        }
        catch (JSException)
        {
            // Ignoruj błędy JS
        }
    }

}