using System.ComponentModel.DataAnnotations;

namespace DineEasy.Application.DTOs.Table;

public class UpdateTableDto
{
    public int Id { get; set; }
    [Required]
    public int TableNumber { get; set; }  
    [Required]
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;        
}