using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
    public Task<bool> HasActiveReservationsAsync(int userId);
    public Task<bool> HasOverlappingReservationAsync(DateTime dateTime, int duration, int tabelId);
    public Task<IEnumerable<Reservation>> GetAllByUserIdAsync(int userId);
}