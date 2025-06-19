namespace DineEasy.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IReservationRepository Reservations { get; }
    ITableRepository Tables { get; }
    ITimeSlotRepository TimeSlots { get; }
    IUserRepository Users { get; }
    IUserProfileRepository UserProfiles { get; }
    Task<int> SaveChangesAsync();
}