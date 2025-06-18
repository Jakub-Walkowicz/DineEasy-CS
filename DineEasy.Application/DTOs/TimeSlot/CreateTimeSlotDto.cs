namespace DineEasy.Application.DTOs.TimeSlot;

public class CreateTimeSlotDto
{
        public TimeOnly StartTime { get; set; }     
        public TimeOnly EndTime { get; set; }        
        public DayOfWeek DayOfWeek { get; set; }    
        public bool IsAvailable { get; set; } = true;
}