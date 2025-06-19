using DineEasy.Application.DTOs.TimeSlot;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using Kiosk.WebAPI.Db.Exceptions;

namespace DineEasy.Application.Services;

public class TimeSlotService(IUnitOfWork unitOfWork) : ITimeSlotService
{
    public async Task<TimeSlotDto?> GetByIdAsync(int id)
    {
        if (id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
        
        var timeslot = await unitOfWork.TimeSlots.GetByIdAsync(id);
        
        return timeslot?.ToDto();
    }

    public async Task<TimeSlotDto> CreateAsync(CreateTimeSlotDto dto)
    {
        var timeSlot = dto.ToEntity();
        
        await unitOfWork.TimeSlots.AddAsync(timeSlot);
        
        await unitOfWork.SaveChangesAsync();
        return timeSlot.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
        
        var timeSlot = await unitOfWork.TimeSlots.GetByIdAsync(id);
        
        if (timeSlot == null) return false;
        
        unitOfWork.TimeSlots.Delete(timeSlot);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateTimeSlotDto dto)
    {
        if (dto.Id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
        
        var existingTimeSlot = await unitOfWork.TimeSlots.GetByIdAsync(dto.Id);
        if (existingTimeSlot == null) return false;

        existingTimeSlot.StartTime = dto.StartTime; 
        existingTimeSlot.EndTime = dto.EndTime;
        existingTimeSlot.DayOfWeek = dto.DayOfWeek;
        
        unitOfWork.TimeSlots.Update(existingTimeSlot);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<TimeSlotDto?> GetByDayOfWeekAsync(DayOfWeek dayOfWeek)
    {
        var timeSlot = await unitOfWork.TimeSlots.GetByDayOfWeekAsync(dayOfWeek);
        return timeSlot?.ToDto();
    }
}