using DineEasy.Application.DTOs.Customer;
using DineEasy.Application.DTOs.Table;
using DineEasy.Domain.Enums;

namespace DineEasy.Application.DTOs.Reservation;

public class ReservationDetailDto
{
    public long Id { get; set; }
    public CustomerDto Customer { get; set; } = null!;
    public TableDto Table { get; set; } = null!;
    public DateTime ReservationDateTime { get; set; }
    public int Duration { get; set; }
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; } 
    public string? SpecialRequests { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}