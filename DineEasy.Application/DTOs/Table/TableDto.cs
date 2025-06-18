namespace DineEasy.Application.DTOs.Table;

public class TableDto
{
    public long Id { get; set; }           
    public int TableNumber { get; set; }  
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;  
    public bool IsActive { get; set; } = true; 
}