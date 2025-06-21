using DineEasy.SharedKernel.Models.Enums;

namespace DineEasy.SharedKernel.Models.Reservation;

public class ReservationDetailDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int UserId { get; set; }           
    public string UserEmail { get; set; } = string.Empty;
    public string? UserFirstName { get; set; } = string.Empty;
    public string? UserLastName { get; set; } = string.Empty;
    public string? UserPhoneNumber { get; set; }
    public int TableId { get; set; }           
    public int TableNumber { get; set; }  
    public int TableCapacity { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int Duration { get; set; }
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; } 
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
}