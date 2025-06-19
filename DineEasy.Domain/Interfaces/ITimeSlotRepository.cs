using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface ITimeSlotRepository : IRepository<TimeSlot>
{
    Task<TimeSlot?> GetByDayOfWeekAsync(DayOfWeek dayOfWeek);
}