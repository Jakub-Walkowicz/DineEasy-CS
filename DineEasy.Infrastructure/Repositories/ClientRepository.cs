using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{

    private readonly DineEasyDbContext _dbContext;

    public ClientRepository(DineEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task AddAsync(Client client)
    {
        await _dbContext.Customers.AddAsync(client);
    }

    public void Update(Client client)
    { 
        _dbContext.Customers.Update(client);
    }

    public void Delete(Client client)
    {
        _dbContext.Customers.Remove(client);
    }

    public async Task<Client?> GetByEmailAsync(string email)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Client?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
}