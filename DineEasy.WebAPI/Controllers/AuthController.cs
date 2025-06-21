using System.Security.Claims;
using DineEasy.Application.Interfaces;
using DineEasy.SharedKernel.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
   [HttpPost("login")]
   public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
   {
       if (!ModelState.IsValid)
           return BadRequest(ModelState);

       try
       {
           logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
           
           var result = await authService.LoginAsync(loginDto);
           
           if (result == null)
           {
               logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
               return Unauthorized("Invalid credentials");
           }

           logger.LogInformation("Successful login for user: {Username}", result.Username);
           return Ok(result);
       }
       catch (Exception ex)
       {
           logger.LogError(ex, "Error during login for email: {Email}", loginDto.Email);
           return StatusCode(500, "An error occurred during login");
       }
   }

   [HttpPost("register")]
   public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
   {
       if (!ModelState.IsValid)
           return BadRequest(ModelState);

       try
       {
           logger.LogInformation("Registration attempt for email: {Email}", registerDto.Email);
           
           var result = await authService.RegisterAsync(registerDto);
           
           if (result == null)
           {
               logger.LogWarning("Failed registration - user already exists: {Email}", registerDto.Email);
               return Conflict("User with this email already exists");
           }

           logger.LogInformation("Successful registration for user: {Username}", result.Username);
           return CreatedAtAction(nameof(Login), result);
       }
       catch (Exception ex)
       {
           logger.LogError(ex, "Error during registration for email: {Email}", registerDto.Email);
           return StatusCode(500, "An error occurred during registration");
       }
   }
   
   [HttpPost("logout")]
   [Authorize]
   public IActionResult Logout()
   {
       try
       {
           var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           var username = User.FindFirst(ClaimTypes.Name)?.Value;
           
           logger.LogInformation("User {Username} (ID: {UserId}) logged out successfully", username, userId);
           
           return Ok(new { message = "Logged out successfully" });
       }
       catch (Exception ex)
       {
           logger.LogError(ex, "Error during logout");
           return StatusCode(500, "An error occurred during logout");
       }
   }
}