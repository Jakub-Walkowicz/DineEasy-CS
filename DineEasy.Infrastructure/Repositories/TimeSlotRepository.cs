using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Repositories;

public class TimeSlotRepository(DineEasyDbContext dbContext) : ITimeSlotRepository
{
    public async Task<TimeSlot?> GetByIdAsync(int id)
    {
        return await dbContext.TimeSlots.FindAsync(id);
    }

    public async Task<IEnumerable<TimeSlot>> GetAllAsync()
    {
        return await dbContext.TimeSlots.ToListAsync();
    }

    public async Task AddAsync(TimeSlot timeSlot)
    {
       await dbContext.TimeSlots.AddAsync(timeSlot);
    }

    public void Update(TimeSlot timeSlot)
    {
        dbContext.TimeSlots.Update(timeSlot);
    }

    public void Delete(TimeSlot timeSlot)
    {
        dbContext.TimeSlots.Remove(timeSlot);
    }

    public async Task<TimeSlot?> GetByDayOfWeekAsync(DayOfWeek dayOfWeek)
    {
        return await dbContext.TimeSlots
            .FirstOrDefaultAsync(x => x!.DayOfWeek == dayOfWeek); 
    }
}