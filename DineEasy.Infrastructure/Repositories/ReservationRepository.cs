using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly DineEasyDbContext _dbContext;

    public ReservationRepository(DineEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Reservation?> GetByIdAsync(long id)
    {
        return await _dbContext.Reservations.FindAsync(id); 
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _dbContext.Reservations.ToListAsync();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _dbContext.Reservations.AddAsync(reservation);
    }

    public void Update(Reservation reservation)
    {
        _dbContext.Reservations.Update(reservation);
    }

    public void Delete(Reservation reservation)
    {
        _dbContext.Reservations.Remove(reservation);
    }

    public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(long customerId)
    {
        return await _dbContext.Reservations
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByTableIdAsync(long tableId)
    {
        return await _dbContext.Reservations
            .Where(r => r.TableId == tableId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByDateAsync(DateTime reservationDateTime)
    {
        return await _dbContext.Reservations
            .Where(r => r.ReservationDateTime.Date == reservationDateTime.Date)
            .ToListAsync();
    }

    public async Task<bool> IsTableAvailableAsync(long tableId, DateTime requestedDateTime, int durationHours = 2)
    {
        var table = await _dbContext.Tables.FindAsync(tableId);
        if (table == null || !table.IsActive)
            return false;
    
        var requestedEndTime = requestedDateTime.AddHours(durationHours);
        
        var hasConflict = await _dbContext.Reservations
            .Where(r => r.TableId == tableId &&
                        r.ReservationDateTime < requestedEndTime &&
                        r.ReservationDateTime.AddHours(r.Duration) > requestedDateTime)
            .AnyAsync();
    
        return !hasConflict; 
    }
}