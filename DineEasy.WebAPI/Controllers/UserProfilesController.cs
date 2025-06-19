using DineEasy.Application.Interfaces;
using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DineEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    private readonly IUnitOfWork _unitOfWork;

    public UserProfileController(IUserProfileService userProfileService, IUnitOfWork unitOfWork)
    {
        _userProfileService = userProfileService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserProfile>> GetById(int id)
    {
        var userProfile = await _userProfileService.GetByIdAsync(id);
        
        if (userProfile == null)
        {
            return NotFound();
        }
        
        return Ok(userProfile);
    }

    [HttpPost]
    public async Task<ActionResult<UserProfile>> Create([FromBody] UserProfile userProfile)
    {
        try
        {
            await _userProfileService.AddAsync(userProfile);
            await _unitOfWork.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = userProfile.Id }, userProfile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserProfile userProfile)
    {
        if (id != userProfile.Id)
        {
            return BadRequest("ID mismatch");
        }

        var existingProfile = await _userProfileService.GetByIdAsync(id);
        if (existingProfile == null)
        {
            return NotFound();
        }

        try
        {
            _userProfileService.Update(userProfile);
            await _unitOfWork.SaveChangesAsync();
            
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userProfile = await _userProfileService.GetByIdAsync(id);
        if (userProfile == null)
        {
            return NotFound();
        }

        _userProfileService.Delete(userProfile);
        await _unitOfWork.SaveChangesAsync();
        
        return NoContent();
    }
}