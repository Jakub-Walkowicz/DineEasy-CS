using DineEasy.Application.DTOs.User;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using Kiosk.WebAPI.Db.Exceptions;
using Microsoft.Extensions.Logging;

namespace DineEasy.Application.Services;

public class UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all users with profiles");
        
        var users = await unitOfWork.Users.GetAllAsync();
        return users.Select(u => u.ToDto());
    }

    public async Task<UserDetailsDto?> GetByIdAsync(int id)
    {
        if (id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
        
        logger.LogInformation("Getting user by ID: {Id}", id);
        
        var user = await unitOfWork.Users.GetByIdAsync(id);
        return user?.ToDetailsDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
        
        logger.LogInformation("Attempting to delete user with ID: {Id}", id);
        
        var user = await unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
        {
            logger.LogWarning("User with ID: {Id} not found for deletion", id);
            return false;
        }
        
        var hasReservations = await unitOfWork.Reservations.HasActiveReservationsAsync(id);
        if (hasReservations)
        {
            logger.LogWarning("Cannot delete user with ID: {Id} - user has active reservations", id);
            throw new BadRequestException("Cannot delete user with active reservations");
        }
        
        unitOfWork.Users.Delete(user);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Successfully deleted user with ID: {Id}", id);

        return true;
    }

    public async Task<bool> UpdateAsync(UserUpdateDto updateDto)
     {
         if (updateDto.Id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
     
         var user = await unitOfWork.Users.GetByIdAsync(updateDto.Id);
         if (user == null)
         {
             logger.LogWarning("User with ID: {Id} not found for update", updateDto.Id);
             return false;
         }
         
         if (user.UserProfile == null)
         {
             user.UserProfile = new UserProfile
             {
                 UserId = user.Id,
                 CreatedAt = DateTime.UtcNow
             };
         }
         
         user.Email = updateDto.Email;
         
         user.UserProfile.FirstName = updateDto.FirstName;
         user.UserProfile.LastName = updateDto.LastName;
         user.UserProfile.PhoneNumber = updateDto.PhoneNumber;
         user.UserProfile.DateOfBirth = updateDto.DateOfBirth;
         
         unitOfWork.Users.Update(user);
         await unitOfWork.SaveChangesAsync();
         logger.LogInformation("Successfully updated user with ID: {Id}", updateDto.Id);
 
         return true;
     }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        logger.LogInformation("Getting a user with email: {email}", email);
        var user = await unitOfWork.Users.GetByEmailAsync(email);
        
        return user?.ToDto();
    }
}