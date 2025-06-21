using DineEasy.Application.Exceptions;
using DineEasy.Application.Interfaces;
using DineEasy.SharedKernel.Models.Reservation;
using Kiosk.WebAPI.Db.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    [HttpGet]
[Authorize(Roles="Admin,User")]
public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll()
{
    try
    {
        _logger.LogInformation("Getting reservations for user: {User}", User.Identity.Name);
        
        foreach (var claim in User.Claims)
        {
            _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
        }
        
        if (User.IsInRole("Admin"))
        {
            _logger.LogInformation("User is Admin - returning all reservations");
            var allReservations = await _reservationService.GetAllAsync();
            return Ok(allReservations);
        }
        else if (User.IsInRole("User"))
        {
            _logger.LogInformation("User is User - returning user reservations");
            
            // Użyj ClaimTypes.NameIdentifier
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null)
            {
                _logger.LogWarning("Could not find user ID claim in token");
                return BadRequest("Invalid user token - no user ID found");
            }
            
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Could not parse user ID: {UserIdValue}", userIdClaim.Value);
                return BadRequest("Invalid user ID format");
            }
            
            _logger.LogInformation("Found user ID: {UserId}", userId);
            var userReservations = await _reservationService.GetAllByUserIdAsync(userId);
            return Ok(userReservations);
        }
        else
        {
            _logger.LogWarning("User has no valid role");
            return Forbid("User does not have required role");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving reservations");
        return StatusCode(500, "An error occurred while retrieving reservations");
    }
}

    [HttpGet("{id}")]
    [Authorize(Roles="Admin,User")]
    public async Task<ActionResult<ReservationDto>> GetById(int id)
    {
        try
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid reservation ID provided: {Id}", id);
                return BadRequest("Reservation ID must be greater than zero");
            }

            _logger.LogInformation("Getting reservation with ID: {Id}", id);
            var reservation = await _reservationService.GetByIdAsync(id);
            
            if (reservation == null)
            {
                _logger.LogWarning("Reservation with ID {Id} not found", id);
                return NotFound($"Reservation with ID {id} not found");
            }

            return Ok(reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservation with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the reservation");
        }
    }

    [HttpPost]
    [Authorize(Roles="Admin,User")]
    public async Task<ActionResult<ReservationDto>> Create(CreateReservationDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for reservation creation: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Could not find or parse user ID from token");
                return BadRequest("Invalid user token");
            }
            
            dto.UserId = userId;

            _logger.LogInformation("Creating new reservation for UserId: {UserId}, TableId: {TableId}", 
                dto.UserId, dto.TableId);
        
            var reservation = await _reservationService.CreateAsync(dto);
        
            _logger.LogInformation("Successfully created reservation with ID: {ReservationId}", reservation!.Id);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating reservation for UserId: {UserId}", dto.UserId);
            return StatusCode(500, "An unexpected error occurred while creating the reservation");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles="Admin,User")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Deleting reservation with ID: {Id}", id);
            var success = await _reservationService.DeleteAsync(id);
            
            if (success)
            {
                _logger.LogInformation("Successfully deleted reservation with ID: {Id}", id);
                return NoContent();
            }
            
            _logger.LogWarning("Reservation with ID {Id} not found for deletion", id);
            return NotFound($"Reservation with ID {id} not found");
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Reservation deletion failed - bad request: {Message}", ex.Message);
            return BadRequest(new { Error = "Invalid request", Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Reservation deletion failed - invalid operation: {Message}", ex.Message);
            return BadRequest(new { Error = "Cannot delete reservation", Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting reservation with ID: {Id}", id);
            return StatusCode(500, "An unexpected error occurred while deleting the reservation");
        }
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetByUserId(int userId)
    {
        try
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID provided: {UserId}", userId);
                return BadRequest("User ID must be greater than zero");
            }

            _logger.LogInformation("Getting reservations for user ID: {UserId}", userId);
            var reservations = await _reservationService.GetAllByUserIdAsync(userId);
        
            _logger.LogDebug("Retrieved {Count} reservations for user ID: {UserId}", 
                reservations.Count(), userId);
        
            return Ok(reservations);
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Get reservations by user failed - bad request: {Message}", ex.Message);
            return BadRequest(new { Error = "Invalid request", Message = ex.Message });
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            _logger.LogWarning("Get reservations by user failed - user not found: {Message}", ex.Message);
            return NotFound(new { Error = "User not found", Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving reservations for user ID: {UserId}", userId);
            return StatusCode(500, "An unexpected error occurred while retrieving user reservations");
        }
    }
}