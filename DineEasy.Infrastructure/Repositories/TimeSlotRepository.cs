using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class TimeSlotRepository : ITimeSlotRepository
{
    
    private readonly DineEasyDbContext _dbContext;

    public TimeSlotRepository(DineEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TimeSlot?> GetByIdAsync(long id)
    {
        return await _dbContext.TimeSlots.FindAsync(id);
    }

    public async Task<IEnumerable<TimeSlot>> GetAllAsync()
    {
        return await _dbContext.TimeSlots.ToListAsync();
    }

    public async Task AddAsync(TimeSlot timeSlot)
    {
       await _dbContext.TimeSlots.AddAsync(timeSlot);
    }

    public void Update(TimeSlot timeSlot)
    {
        _dbContext.TimeSlots.Update(timeSlot);
    }

    public void Delete(TimeSlot timeSlot)
    {
        _dbContext.TimeSlots.Remove(timeSlot);
    }

    public async Task<IEnumerable<TimeSlot>> GetAvailableAsync()
    {
        return await _dbContext.TimeSlots.Where(x => x.IsAvailable).ToListAsync();
    }

    public async Task<IEnumerable<TimeSlot>> GetAvailableByDayOfWeekAsync(DayOfWeek dayOfWeek)
    {
        return await _dbContext.TimeSlots.Where(x => x.DayOfWeek == dayOfWeek && x.IsAvailable).ToListAsync(); 
    }
}