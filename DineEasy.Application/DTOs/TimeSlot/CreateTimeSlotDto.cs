using System.ComponentModel.DataAnnotations;

namespace DineEasy.Application.DTOs.TimeSlot;

public class CreateTimeSlotDto
{
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }    
}