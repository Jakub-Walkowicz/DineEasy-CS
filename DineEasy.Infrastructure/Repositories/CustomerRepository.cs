using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{

    private readonly DineEasyDbContext _dbContext;

    public CustomerRepository(DineEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Customer?> GetByIdAsync(long id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
    }

    public void Update(Customer customer)
    { 
        _dbContext.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
}