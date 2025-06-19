using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class UserRepository(DineEasyDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users
            .Include(u => u.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync(); 
    }
    
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }

    public void Delete(User user)
    {
        context.Users.Remove(user);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}