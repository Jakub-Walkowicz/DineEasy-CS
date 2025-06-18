using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class TableRepository : ITableRepository
{

    private readonly DineEasyDbContext _dbContext;

    public TableRepository(DineEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Table?> GetByIdAsync(long id)
    {
        return await _dbContext.Tables.FindAsync(id);
    }

    public async Task<IEnumerable<Table>> GetAllAsync()
    {
        return await _dbContext.Tables.ToListAsync();
    }

    public async Task AddAsync(Table table)
    {
        await _dbContext.Tables.AddAsync(table);
    }

    public void Update(Table table)
    {
        _dbContext.Tables.Update(table);
    }

    public void Delete(Table table)
    {
        _dbContext.Tables.Remove(table);
    }

    public async Task<IEnumerable<Table>> GetAllByCapacityAsync(int capacity)
    {
        return await _dbContext.Tables.Where(x => x.Capacity == capacity).ToListAsync();
    }

    public async Task<IEnumerable<Table>> GetAllByLocationAsync(string location)
    {
        return await _dbContext.Tables.Where(x => x.Location == location).ToListAsync();
    }
}