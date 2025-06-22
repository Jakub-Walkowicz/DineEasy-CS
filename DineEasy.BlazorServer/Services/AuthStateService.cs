using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DineEasy.BlazorServer.Services;

public class AuthStateService
{
    private readonly IJSRuntime _jsRuntime;
    private string? _token;
    
    public bool IsAuthenticated { get; private set; }
    public string? Username { get; private set; }
    public string? Token => _token;
    public bool IsAdmin { get; private set; }
    public List<string> Roles { get; private set; } = new();
    
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
        
        // Dekoduj token JWT i wyciągnij role
        ExtractRolesFromToken(token);
        
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
        IsAdmin = false;
        Roles.Clear();
        
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
                // Sprawdź czy token nie wygasł
                if (IsTokenValid(token))
                {
                    IsAuthenticated = true;
                    Username = username;
                    _token = token;
                    
                    // Dekoduj token JWT i wyciągnij role
                    ExtractRolesFromToken(token);
                    
                    OnChange?.Invoke();
                }
                else
                {
                    // Token wygasł, wyloguj użytkownika
                    await SetUnauthenticatedAsync();
                }
            }
        }
        catch (JSException)
        {
            // Ignoruj błędy JS
        }
    }

    private void ExtractRolesFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            
            // Wyciągnij role z claims
            Roles = jsonToken.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .ToList();
            
            // Sprawdź czy użytkownik jest adminem
            IsAdmin = Roles.Contains("Admin");
        }
        catch (Exception)
        {
            // Jeśli nie udało się zdekodować tokenu, ustaw domyślne wartości
            Roles = new List<string>();
            IsAdmin = false;
        }
    }

    private bool IsTokenValid(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            
            // Sprawdź czy token nie wygasł
            return jsonToken.ValidTo > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    public bool HasRole(string role)
    {
        return Roles.Contains(role);
    }
}