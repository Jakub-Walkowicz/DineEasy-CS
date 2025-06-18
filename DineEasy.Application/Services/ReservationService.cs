using DineEasy.Application.DTOs.Reservation;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Entities;
using DineEasy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
        return reservation?.ToDto();
    }

    public async Task<ReservationDto?> CreateAsync(CreateReservationDto dto)
    {
        
        var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId); 
        if (customer == null) throw new ArgumentException($"Customer with ID {dto.CustomerId} not found");
        
        var table = await _unitOfWork.Tables.GetByIdAsync(dto.TableId);
        if (table == null) throw new Exception($"Table with ID {dto.TableId} not found");
        
        var isAvailable = await _unitOfWork.Reservations.IsTableAvailableAsync(table.Id, dto.ReservationDateTime, dto.Duration);
        if (!isAvailable) throw new Exception($"Table is not available for reservation");
        
        try
        {
            var reservation = dto.ToEntity();
            await _unitOfWork.Reservations.AddAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
            return reservation.ToDto();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Reservations_NoOverlap") == true)
        {
            throw new InvalidOperationException("Table is no longer available - another reservation was created");
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
        if (reservation == null) return false;
        
        if (reservation.ReservationDateTime <= DateTime.Now.AddHours(1))
            throw new InvalidOperationException("Cannot cancel reservation less than 1 hour before scheduled time");
        
        _unitOfWork.Reservations.Delete(reservation);
        
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<ReservationDto?>> GetAllAsync()
    {
        var reservations = await _unitOfWork.Reservations.GetAllAsync();
        return reservations.ToDtos();
    }
}