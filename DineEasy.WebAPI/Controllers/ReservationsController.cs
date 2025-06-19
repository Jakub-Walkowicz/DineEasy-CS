using DineEasy.Application.DTOs.Reservation;
using DineEasy.Application.Interfaces;
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
    public async Task<ActionResult<List<ReservationDto>>> GetAll()
    {
        _logger.LogInformation("Getting all reservations");
        var reservations = await _reservationService.GetAllAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetById(int id)
    {
        _logger.LogInformation("Getting reservation with ID: {Id}", id);
        var reservation = await _reservationService.GetByIdAsync(id);
        return reservation == null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> Create(CreateReservationDto dto)
    {
        try
        {
            _logger.LogInformation("Creating new reservation for client: {ClientId}", dto.ClientId);
            var reservation = await _reservationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reservation!.Id }, reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Deleting reservation with ID: {Id}", id);
            var success = await _reservationService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reservation with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}  