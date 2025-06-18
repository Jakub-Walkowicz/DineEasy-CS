using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;

namespace DineEasy.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DineEasyDbContext _dbContext;
    
    private ICustomerRepository? _customers;
    private IReservationRepository? _reservations;
    private ITableRepository? _tables;
    private ITimeSlotRepository? _timeSlots;

    public UnitOfWork(DineEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public ICustomerRepository Customers =>
        _customers ??= new CustomerRepository(_dbContext);
        
    public IReservationRepository Reservations =>
        _reservations ??= new ReservationRepository(_dbContext);
        
    public ITableRepository Tables =>
        _tables ??= new TableRepository(_dbContext);
        
    public ITimeSlotRepository TimeSlots =>
        _timeSlots ??= new TimeSlotRepository(_dbContext);
    
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}