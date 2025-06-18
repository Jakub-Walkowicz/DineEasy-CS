using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(long id);
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task AddAsync(Reservation reservation);
    void Update(Reservation reservation);
    void Delete(Reservation reservation);
    
    Task<IEnumerable<Reservation>> GetByCustomerIdAsync(long customerId);
    Task<IEnumerable<Reservation>> GetByTableIdAsync(long tableId);
    Task<IEnumerable<Reservation>> GetByDateAsync(DateTime date);
    Task<bool> IsTableAvailableAsync(long tableId, DateTime requestedDateTime, int durationHours = 2);
}