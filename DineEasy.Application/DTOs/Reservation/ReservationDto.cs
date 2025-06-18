using DineEasy.Domain.Enums;

namespace DineEasy.Application.DTOs.Reservation;

public class ReservationDto
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long TableId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int Duration { get; set; }
    public ReservationStatus Status { get; set; }
}