using DineEasy.Application.DTOs.TimeSlot;

namespace DineEasy.Application.Interfaces;

public interface ITimeSlotService
{
    Task<TimeSlotDto?> GetByIdAsync(int id);
    Task<TimeSlotDto> CreateAsync(CreateTimeSlotDto dto);
    Task<TimeSlotDto?> GetByDayOfWeekAsync (DayOfWeek dayOfWeek);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(UpdateTimeSlotDto dto);
}