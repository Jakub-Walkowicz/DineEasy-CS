using DineEasy.Domain.Entities;
using DineEasy.SharedKernel.Models.Reservation;

namespace DineEasy.Application.Extensions;

public static class ReservationExtensions
{
    public static ReservationDto ToDto(this Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            TableNumber = reservation.Table.TableNumber,
            ReservationDateTime = reservation.ReservationDateTime,
            Status = reservation.Status,
            UserEmail = reservation.User.Email,
            Duration = reservation.Duration 
        };
    } 
    
    public static Reservation ToEntity(this CreateReservationDto dto)
    {
        return new Reservation
        {
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
        return reservations.Select(r => r.ToDto()).ToList();
    }

    public static ReservationDetailDto ToDetailsDto(this Reservation reservation)
    {
        return new ReservationDetailDto
        {
            Id = reservation.Id,
            
            Username = reservation.User.Username,
            UserId = reservation.UserId,
            UserEmail = reservation.User.Email,
            UserFirstName = reservation.User.UserProfile?.FirstName,
            UserLastName = reservation.User.UserProfile?.LastName,
            UserPhoneNumber = reservation.User.UserProfile?.PhoneNumber,
            
            TableId = reservation.TableId,
            TableNumber = reservation.Table.TableNumber,
            TableCapacity = reservation.Table.Capacity,
            
            ReservationDateTime = reservation.ReservationDateTime,
            Duration = reservation.Duration,
            PartySize = reservation.PartySize,
            Status = reservation.Status,
            SpecialRequests = reservation.SpecialRequests,
            CreatedAt = reservation.CreatedAt
        };
    }
} 