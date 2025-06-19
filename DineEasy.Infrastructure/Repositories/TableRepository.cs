using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class TableRepository(DineEasyDbContext dbContext) : ITableRepository
{
    public async Task<Table?> GetByIdAsync(int id)
    {
        return await dbContext.Tables.FindAsync(id);
    }

    public async Task<IEnumerable<Table>> GetAllAsync()
    {
        return await dbContext.Tables.ToListAsync();
    }

    public async Task AddAsync(Table table)
    {
        await dbContext.Tables.AddAsync(table);
    }

    public void Update(Table table)
    {
        dbContext.Tables.Update(table);
    }

    public void Delete(Table table)
    {
        dbContext.Tables.Remove(table);
    }
}