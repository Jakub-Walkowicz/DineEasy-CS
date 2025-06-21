using DineEasy.BlazorServer.Services;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly AuthStateService _authStateService;

    public AuthTokenHandler(AuthStateService authStateService)
    {
        _authStateService = authStateService;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        if (_authStateService.IsAuthenticated && !string.IsNullOrEmpty(_authStateService.Token))
        {
            request.Headers.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authStateService.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}