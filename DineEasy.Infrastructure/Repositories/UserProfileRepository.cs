using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class UserProfileRepository(DineEasyDbContext context) : IUserProfileRepository
{
    public async Task<UserProfile?> GetByIdAsync(int id)
    {
        return await context.UserProfiles.FindAsync(id);
    }

    public async Task<IEnumerable<UserProfile>> GetAllAsync()
    {
        return await context.UserProfiles.ToListAsync();
    }

    public async Task AddAsync(UserProfile userProfile)
    {
        await context.UserProfiles.AddAsync(userProfile);
    }

    public void Update(UserProfile userProfile)
    {
        context.UserProfiles.Update(userProfile);
    }

    public void Delete(UserProfile userProfile)
    {
        context.UserProfiles.Remove(userProfile);
    }
}