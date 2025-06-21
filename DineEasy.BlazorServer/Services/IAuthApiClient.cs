using DineEasy.SharedKernel.Models.Auth;

public interface IAuthApiClient
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
}