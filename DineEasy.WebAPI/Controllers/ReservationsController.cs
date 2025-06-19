using DineEasy.Application.DTOs.Reservation;
using DineEasy.Application.Exceptions;
using DineEasy.Application.Interfaces;
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
    [Authorize(Roles="Admin")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Getting all reservations");
            var reservations = await _reservationService.GetAllAsync();
            _logger.LogDebug("Retrieved {Count} reservations", reservations.Count());
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all reservations");
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

            _logger.LogInformation("Creating new reservation for UserId: {UserId}, TableId: {TableId}", 
                dto.UserId, dto.TableId);
            
            var reservation = await _reservationService.CreateAsync(dto);
            
            _logger.LogInformation("Successfully created reservation with ID: {ReservationId}", reservation!.Id);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (ReservationHoursOutOfBoundException ex)
        {
            _logger.LogWarning("Reservation creation failed - hours out of bound: {Message}", ex.Message);
            return BadRequest(new { Error = "Invalid reservation time", Message = ex.Message });
        }
        catch (UnavailableTableException ex)
        {
            _logger.LogWarning("Reservation creation failed - table unavailable: {Message}", ex.Message);
            return Conflict(new { Error = "Table unavailable", Message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Reservation creation failed - bad request: {Message}", ex.Message);
            return BadRequest(new { Error = "Invalid request", Message = ex.Message });
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
}