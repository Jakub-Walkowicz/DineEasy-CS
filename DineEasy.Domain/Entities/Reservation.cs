using DineEasy.Domain.Enums;

namespace DineEasy.Domain.Entities;

public class Reservation
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long TableId { get; set; }
    public DateTime ReservationDateTime { get; set; } = DateTime.UtcNow;
    public int Duration { get; set; } = 2;
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending; 
    public string SpecialRequests { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}