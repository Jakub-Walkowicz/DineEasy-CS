using DineEasy.SharedKernel.Models.Reservation;

namespace DineEasy.BlazorServer.Services
{
    public interface IReservationApiClient
    {
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
        Task<ReservationDto?> GetReservationByIdAsync(int id);
        Task<ReservationDto?> CreateReservationAsync(CreateReservationDto dto);
        Task<bool> DeleteReservationAsync(int id);
    }
}