using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync (string username);
    Task<User?> GetByEmailAsync(string email);
}