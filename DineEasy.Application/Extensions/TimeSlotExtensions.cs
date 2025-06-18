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
            IsAvailable = timeSlot.IsAvailable,
            StartTime = timeSlot.StartTime,
            EndTime = timeSlot.EndTime
        };
    }

    public static TimeSlot ToEntity(this CreateTimeSlotDto dto)
    {
        return new TimeSlot
        {
            DayOfWeek = dto.DayOfWeek,
            IsAvailable = dto.IsAvailable,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };
    }

    public static List<TimeSlotDto> ToDtos(this IEnumerable<TimeSlot> timeSlots)
    {
        return timeSlots.Select(timeSlot => timeSlot.ToDto()).ToList();
    }
}