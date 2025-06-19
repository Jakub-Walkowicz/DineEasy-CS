using DineEasy.Application.DTOs.Table;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly ITableService _tableService;
    private readonly ILogger<TablesController> _logger;

    public TablesController(ITableService tableService, ILogger<TablesController> logger)
    {
        _tableService = tableService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<TableDto>>> GetAll()
    {
        _logger.LogInformation("Getting all tables");
        var tables = await _tableService.GetAllAsync();
        return Ok(tables);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableDto>> GetById(int id)
    {
        _logger.LogInformation("Getting table with ID: {Id}", id);
        // You'll need to add this method to ITableService
        return Ok(new { message = "GetById not implemented yet" });
    }

    [HttpPost]
    public async Task<ActionResult<TableDto>> Create(CreateTableDto dto)
    {
        try
        {
            _logger.LogInformation("Creating new table number: {TableNumber}", dto.TableNumber);
            var table = await _tableService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = table.Id }, table);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating table");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Deleting table with ID: {Id}", id);
            var success = await _tableService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting table with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}