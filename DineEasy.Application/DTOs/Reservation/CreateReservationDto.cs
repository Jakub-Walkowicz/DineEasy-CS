using System.ComponentModel.DataAnnotations;
using DineEasy.Domain.Enums;

namespace DineEasy.Application.DTOs.Reservation;

public class CreateReservationDto
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int TableId { get; set; }
    [Required]
    public DateTime ReservationDateTime { get; set; }
    [Required]
    public int Duration { get; set; } = 2;
    [Required]
    public int PartySize { get; set; }
    [MaxLength(500)]
    public string? SpecialRequests { get; set; } = string.Empty;
}