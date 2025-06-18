using DineEasy.Application.DTOs.TimeSlot;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Interfaces;

namespace DineEasy.Application.Services;

public class TimeSlotService : ITimeSlotService
{
    
    private readonly IUnitOfWork _unitOfWork;

    public TimeSlotService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<TimeSlotDto>> GetAllAsync()
    {
        var timeSlots = await _unitOfWork.TimeSlots.GetAllAsync();
        return timeSlots.ToDtos();
    }

    public async Task<TimeSlotDto> CreateAsync(CreateTimeSlotDto dto)
    {
        var timeSlot = dto.ToEntity();
        await _unitOfWork.TimeSlots.AddAsync(timeSlot);
        await _unitOfWork.SaveChangesAsync();
        return timeSlot.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var timeSlot = await _unitOfWork.TimeSlots.GetByIdAsync(id);
        if (timeSlot == null) return false;
        _unitOfWork.TimeSlots.Delete(timeSlot);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}