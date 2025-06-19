using DineEasy.Application.DTOs.User;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController(IUserService userService, ILogger<UsersController> logger)
    : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        logger.LogInformation("Getting all users");
        var users = await userService.GetAllAsync();
        return Ok(users.ToList());
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<UserDetailsDto>> GetById(int id)
    {
        try
        {
            logger.LogInformation("Getting user with ID: {Id}", id);
            var user = await userService.GetByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user with ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            logger.LogInformation("Deleting user with ID: {Id}", id);
            var success = await userService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting user with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult> Update(int id, [FromBody] UserUpdateDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch between route and body!");
            }
            logger.LogInformation("Updating user with ID: {Id}", id);
            var success = await userService.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user with ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}