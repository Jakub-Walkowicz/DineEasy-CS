using DineEasy.Domain.Entities;
using DineEasy.Domain.Enums;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using DineEasy.SharedKernel.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class ReservationRepository(DineEasyDbContext dbContext) : IReservationRepository
{
    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await dbContext.Reservations
            .Include(r => r.User)
            .Include(r => r.Table)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await dbContext.Reservations
            .Include(r => r.Table)     
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await dbContext.Reservations.AddAsync(reservation);
    }

    public void Update(Reservation reservation)
    {
        dbContext.Reservations.Update(reservation);
    }

    public void Delete(Reservation reservation)
    {
        dbContext.Reservations.Remove(reservation);
    }

    public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int clientId)
    {
        return await dbContext.Reservations
            .Where(r => r.UserId == clientId)
            .ToListAsync();
    }
    
    public async Task<bool> HasActiveReservationsAsync(int userId)
    {
        return await dbContext.Reservations
            .AnyAsync(r => r.UserId == userId && 
                           (r.Status == ReservationStatus.Confirmed || 
                            r.Status == ReservationStatus.Pending));
    }

    public async Task<bool> HasOverlappingReservationAsync(DateTime dateTime, int duration, int tabelId)
    {
        var newReservationEnd = dateTime.AddHours(duration);
        
        return await dbContext.Reservations
            .Where(r => r.TableId == tabelId)
            .Where(r => r.Status != ReservationStatus.Cancelled 
                        && r.Status != ReservationStatus.Completed)
            .AnyAsync(r => 
                dateTime < r.ReservationDateTime.AddHours(r.Duration) &&
                newReservationEnd > r.ReservationDateTime);
    }

    public async Task<IEnumerable<Reservation>> GetAllByUserIdAsync(int userId)
    {
        return await dbContext.Reservations
            .Include(r => r.Table)    
            .Include(r => r.User)     
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }
}