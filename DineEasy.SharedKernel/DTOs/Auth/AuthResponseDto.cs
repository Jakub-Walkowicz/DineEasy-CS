namespace DineEasy.SharedKernel.Models.Auth;

public class AuthResponseDto
{
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int UserId { get; set; }
}