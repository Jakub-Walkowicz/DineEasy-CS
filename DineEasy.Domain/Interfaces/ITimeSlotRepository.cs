using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface ITimeSlotRepository
{
    Task<TimeSlot?> GetByIdAsync(long id);
    Task<IEnumerable<TimeSlot>> GetAllAsync();
    Task AddAsync(TimeSlot timeSlot);
    void Update(TimeSlot timeSlot);
    void Delete(TimeSlot timeSlot);
    
    Task<IEnumerable<TimeSlot>> GetAvailableAsync();
    Task<IEnumerable<TimeSlot>> GetAvailableByDayOfWeekAsync(DayOfWeek dayOfWeek);
}