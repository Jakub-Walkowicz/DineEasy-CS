using DineEasy.Application.DTOs.Client;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientService clientService, ILogger<ClientsController> logger)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
    {
        try
        {
            logger.LogInformation("Creating new client with email: {Email}", dto.Email);
            var client = await clientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating client");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        logger.LogInformation("Getting client with ID: {Id}", id);
        var client = await clientService.GetByIdAsync(id);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            logger.LogInformation("Deleting client with ID: {Id}", id);
            var success = await clientService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting client with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}