using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Entities;
using DineEasy.Domain.Enums;
using DineEasy.Domain.Interfaces;
using DineEasy.SharedKernel.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DineEasy.Application.Services;

public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<AuthService> logger) 
    : IAuthService
{
    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
        
        var user = await unitOfWork.Users.GetByEmailAsync(loginDto.Email);
        
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
            return null;
        }

        var token = GenerateJwtToken(user);
        
        logger.LogInformation("Successful login for user: {Username}", user.Username);
        
        return new AuthResponseDto
        {
            Username = user.Username,
            Token = token,
            Role = user.Role.ToString(),
            Email = user.Email
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        logger.LogInformation("Registration attempt for email: {Email}", registerDto.Email);
        
        if (await unitOfWork.Users.EmailExistsAsync(registerDto.Email))
        {
            logger.LogWarning("Registration failed - email already exists: {Email}", registerDto.Email);
            return null;
        }

        if (await unitOfWork.Users.UsernameExistsAsync(registerDto.Username))
        {
            logger.LogWarning("Registration failed - username already exists: {Username}", registerDto.Username);
            return null;
        }
        
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = HashPassword(registerDto.Password),
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow,
        };
        
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();
        
        var userProfile = new UserProfile
        {
            UserId = user.Id,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber,
            DateOfBirth = registerDto.DateOfBirth,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.UserProfiles.AddAsync(userProfile);
        await unitOfWork.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        
        logger.LogInformation("Successful registration for user: {Username}", user.Username);
        
        return new AuthResponseDto
        {
            Username = user.Username,
            Token = token,
            Role = user.Role.ToString(),
            Email = user.Email
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()), // "Admin" or "User"
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}