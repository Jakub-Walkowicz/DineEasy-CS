using DineEasy.Application.DTOs.TimeSlot;

namespace DineEasy.Application.Interfaces;

public interface ITimeSlotService
{
    Task<List<TimeSlotDto>> GetAllAsync();
    Task<TimeSlotDto> CreateAsync(CreateTimeSlotDto dto);
    Task<bool> DeleteAsync(int id);
}