using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface ITableRepository
{
    Task<Table?> GetByIdAsync(long id);
    Task<IEnumerable<Table>> GetAllAsync();
    Task AddAsync(Table table);
    void Update(Table table);
    void Delete(Table table);
    
    Task<IEnumerable<Table>> GetAllByCapacityAsync(int capacity);
    Task<IEnumerable<Table>> GetAllByLocationAsync(string location);
}