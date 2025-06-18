using DineEasy.Application.DTOs.Reservation;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Extensions;

public static class ReservationExtensions
{
    public static ReservationDto? ToDto(this Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            ReservationDateTime = reservation.ReservationDateTime,
            Status = reservation.Status,
            TableId = reservation.TableId,
            Duration = reservation.Duration // Add this line
        };
    } 
    
    public static Reservation ToEntity(this CreateReservationDto dto)
    {
        return new Reservation
        {
            CustomerId = dto.CustomerId,
            PartySize = dto.PartySize,
            ReservationDateTime = dto.ReservationDateTime,
            Duration = dto.Duration,
            SpecialRequests = dto.SpecialRequests ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            TableId = dto.TableId
        };
    }

    public static List<ReservationDto> ToDtos(this IEnumerable<Reservation> reservations)
    {
        return reservations.Select(r => r.ToDto()).Where(dto => dto != null).Cast<ReservationDto>().ToList();
    }
}