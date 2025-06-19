using DineEasy.Domain.Enums;

namespace DineEasy.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TableId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int Duration { get; set; } = 2;
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending; 
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Table Table { get; set; } = null!;
}