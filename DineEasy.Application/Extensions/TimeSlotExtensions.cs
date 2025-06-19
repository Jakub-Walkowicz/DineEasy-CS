using DineEasy.Application.DTOs.TimeSlot;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Extensions;

public static class TimeSlotExtensions
{
    public static TimeSlotDto ToDto(this TimeSlot timeSlot)
    {
        return new TimeSlotDto
        {
            Id = timeSlot.Id,
            DayOfWeek = timeSlot.DayOfWeek,
            StartTime = timeSlot.StartTime,
            EndTime = timeSlot.EndTime
        };
    }

    public static TimeSlot ToEntity(this CreateTimeSlotDto dto)
    {
        return new TimeSlot
        {
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };
    }
}