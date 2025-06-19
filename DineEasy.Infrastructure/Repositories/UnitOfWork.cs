using DineEasy.Domain.Interfaces;
using DineEasy.Infrastructure.Data;

namespace DineEasy.Infrastructure.Repositories;

public class UnitOfWork(DineEasyDbContext dbContext) : IUnitOfWork
{
    private IReservationRepository? _reservations;
    private ITableRepository? _tables;
    private ITimeSlotRepository? _timeSlots;
    private IUserRepository? _users;
    private IUserProfileRepository? _userProfiles;

    public IUserProfileRepository UserProfiles => 
        _userProfiles ??= new UserProfileRepository(dbContext);
    public IUserRepository Users => 
        _users ??= new UserRepository(dbContext);
        
    public IReservationRepository Reservations =>
        _reservations ??= new ReservationRepository(dbContext);
        
    public ITableRepository Tables =>
        _tables ??= new TableRepository(dbContext);
        
    public ITimeSlotRepository TimeSlots =>
        _timeSlots ??= new TimeSlotRepository(dbContext);
    
    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        dbContext?.Dispose();
    }
}