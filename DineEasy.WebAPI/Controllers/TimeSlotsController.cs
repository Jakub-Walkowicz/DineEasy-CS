using DineEasy.Application.DTOs.TimeSlot;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeSlotsController : ControllerBase
{
    private readonly ITimeSlotService _timeSlotService;
    private readonly ILogger<TimeSlotsController> _logger;

    public TimeSlotsController(ITimeSlotService timeSlotService, ILogger<TimeSlotsController> logger)
    {
        _timeSlotService = timeSlotService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeSlotDto>> GetById(int id)
    {
        _logger.LogInformation("Getting time slot with ID: {Id}", id);
        return Ok(new { message = "GetById not implemented yet" });
    }

    [HttpPost]
    public async Task<ActionResult<TimeSlotDto>> Create(CreateTimeSlotDto dto)
    {
        try
        {
            _logger.LogInformation("Creating new time slot: {StartTime} - {EndTime}", dto.StartTime, dto.EndTime);
            var timeSlot = await _timeSlotService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = timeSlot.Id }, timeSlot);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating time slot");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Deleting time slot with ID: {Id}", id);
            var success = await _timeSlotService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting time slot with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}