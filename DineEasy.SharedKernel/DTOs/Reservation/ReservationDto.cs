using DineEasy.SharedKernel.Models.Enums;

namespace DineEasy.SharedKernel.Models.Reservation;

public class ReservationDto
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public int TableNumber { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int Duration { get; set; }
    public ReservationStatus Status { get; set; }
}