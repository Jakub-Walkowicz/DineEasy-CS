using DineEasy.Application.DTOs.Reservation;
using DineEasy.Application.DTOs.Table;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Interfaces;

public interface IReservationService
{
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<ReservationDto?> CreateAsync(CreateReservationDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ReservationDto>> GetAllAsync();
    Task<IEnumerable<ReservationDto>> GetAllByUserIdAsync(int userId);
}