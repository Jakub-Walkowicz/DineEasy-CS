namespace DineEasy.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IReservationRepository Reservations { get; }
    ITableRepository Tables { get; }
    ITimeSlotRepository TimeSlots { get; }

    Task<int> SaveChangesAsync();
}