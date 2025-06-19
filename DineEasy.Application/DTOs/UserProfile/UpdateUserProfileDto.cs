namespace DineEasy.Application.DTOs.TimeSlot;

public class UpdateUserProfileDto
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }     
    public TimeOnly EndTime { get; set; }        
    public DayOfWeek DayOfWeek { get; set; }    
}