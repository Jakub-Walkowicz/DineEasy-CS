using DineEasy.Domain.Enums;

namespace DineEasy.Application.DTOs.Reservation;

public class CreateReservationDto
{
    public long CustomerId { get; set; }
    public long TableId { get; set; }
    public DateTime ReservationDateTime { get; set; } = DateTime.UtcNow;
    public int Duration { get; set; } = 2;
    public int PartySize { get; set; }
    public string? SpecialRequests { get; set; } = string.Empty;
}