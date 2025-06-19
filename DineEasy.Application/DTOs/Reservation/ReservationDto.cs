using DineEasy.Domain.Enums;

namespace DineEasy.Application.DTOs.Reservation;

public class ReservationDto
{
    public int Id { get; set; }
    public string UserEmail { get; set; }
    public int TableNumber { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int Duration { get; set; }
    public ReservationStatus Status { get; set; }
}