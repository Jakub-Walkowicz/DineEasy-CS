using DineEasy.Application.DTOs.TimeSlot;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeSlotsController(ITimeSlotService timeSlotService, ILogger<TimeSlotsController> logger)
    : ControllerBase
{
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<TimeSlotDto>> GetById(int id)
    {
        try
        {
            logger.LogInformation("Getting time slot with ID: {Id}", id);
            var timeSlot = await timeSlotService.GetByIdAsync(id);
            return timeSlot != null ? Ok(timeSlot) : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting time slot with ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TimeSlotDto>> Create(CreateTimeSlotDto dto)
    {
        try
        {
            logger.LogInformation("Creating new time slot: {StartTime} - {EndTime}", dto.StartTime, dto.EndTime);
            var timeSlot = await timeSlotService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = timeSlot.Id }, timeSlot);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating time slot");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            logger.LogInformation("Deleting time slot with ID: {Id}", id);
            var success = await timeSlotService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting time slot with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTimeSlotDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch between route and body!");
            }
            logger.LogInformation("Updating time slot with ID: {Id}", id);
            var success = await timeSlotService.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating time slot with ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("day/{dayOfWeek}")]
    [Authorize(Roles = "Admin,User" )]
    public async Task<ActionResult<TimeSlotDto>> GetByDayOfWeek(DayOfWeek dayOfWeek)
    {
        try
        {
            logger.LogInformation("Getting time slot for day: {DayOfWeek}", dayOfWeek);
            var timeSlot = await timeSlotService.GetByDayOfWeekAsync(dayOfWeek);
            return timeSlot != null ? Ok(timeSlot) : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting time slot for day: {DayOfWeek}", dayOfWeek);
            return StatusCode(500, "Internal server error");
        }
    }
}