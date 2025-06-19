using DineEasy.Application.DTOs.Customer;
using DineEasy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
    {
        try
        {
            logger.LogInformation("Creating new customer with email: {Email}", dto.Email);
            var customer = await customerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating customer");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        logger.LogInformation("Getting customer with ID: {Id}", id);
        var customer = await customerService.GetByIdAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            logger.LogInformation("Deleting customer with ID: {Id}", id);
            var success = await customerService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting customer with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}