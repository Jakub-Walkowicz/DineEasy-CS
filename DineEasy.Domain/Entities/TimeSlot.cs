namespace DineEasy.Domain.Entities;

public class TimeSlot
{
    public long Id { get; set; }
    public TimeOnly StartTime { get; set; }     
    public TimeOnly EndTime { get; set; }        
    public DayOfWeek DayOfWeek { get; set; }    
    public bool IsAvailable { get; set; } = true;
}