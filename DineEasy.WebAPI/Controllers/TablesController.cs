using DineEasy.Application.DTOs.Table;
using DineEasy.Application.Interfaces;
using DineEasy.SharedKernel.Models.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class TablesController(ITableService tableService, ILogger<TablesController> logger)
    : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<List<TableDto>>> GetAll()
    {
        logger.LogInformation("Getting all tables");
        var tables = await tableService.GetAllAsync();
        return Ok(tables);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<TableDto>> GetById(int id)
    {
        try
        {
            logger.LogInformation("Getting table with ID: {Id}", id);
            var table = await tableService.GetByIdAsync(id);
            return table != null ? Ok(table) : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting table with ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TableDto>> Create(CreateTableDto dto)
    {
        try
        {
            logger.LogInformation("Creating new table number: {TableNumber}", dto.TableNumber);
            var table = await tableService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = table.Id }, table);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating table");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            logger.LogInformation("Deleting table with ID: {Id}", id);
            var success = await tableService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting table with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTableDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch between route and body!");
            }
            logger.LogInformation("Updating table with ID: {Id}", id);
            var success = await tableService.UpdateAsync(dto);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating table with ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}