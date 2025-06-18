namespace DineEasy.Domain.Entities;

public class Table
{
    public long Id { get; set; }           
    public int TableNumber { get; set; }  
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;  
    public bool IsActive { get; set; } = false;           
}