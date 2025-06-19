using DineEasy.Application.DTOs.User;

namespace DineEasy.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDetailsDto?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(UserUpdateDto updateDto);
    Task<UserDto?> GetByEmailAsync(string email);
}