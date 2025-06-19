using DineEasy.Application.DTOs.Auth;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
            
            var result = await authService.LoginAsync(loginDto);
            
            if (result == null)
            {
                logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
                return Unauthorized("Invalid email or password");
            }

            logger.LogInformation("Successful login for user: {Username}", result.Username);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during login");
            return BadRequest("An error occurred during login");
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        try
        {
            logger.LogInformation("Registration attempt for email: {Email}", registerDto.Email);
            
            var result = await authService.RegisterAsync(registerDto);
            
            if (result == null)
            {
                logger.LogWarning("Failed registration - user already exists: {Email}", registerDto.Email);
                return BadRequest("User with this email already exists");
            }

            logger.LogInformation("Successful registration for user: {Username}", result.Username);
            return CreatedAtAction(nameof(Login), result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during registration");
            return BadRequest("An error occurred during registration");
        }
    }
}