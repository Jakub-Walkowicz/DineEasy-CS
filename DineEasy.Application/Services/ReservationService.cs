
using DineEasy.Application.DTOs.Reservation;
using DineEasy.Application.Exceptions;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Interfaces;
using Kiosk.WebAPI.Db.Exceptions;
using Microsoft.Extensions.Logging;

namespace DineEasy.Application.Services;

public class ReservationService(IUnitOfWork unitOfWork, ILogger<ReservationService> logger) : IReservationService
{
    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Attempting to retrieve reservation with ID: {ReservationId}", id);
        
        var reservation = await unitOfWork.Reservations.GetByIdAsync(id);
        
        if (reservation == null)
        {
            logger.LogWarning("Reservation with ID {ReservationId} not found", id);
            return null;
        }
        
        logger.LogInformation("Successfully retrieved reservation with ID: {ReservationId}", id);
        return reservation?.ToDto();
    }

    public async Task<ReservationDto?> CreateAsync(CreateReservationDto dto)
    {
        logger.LogInformation("Creating new reservation - UserId: {UserId}, TableId: {TableId}, DateTime: {DateTime}, Duration: {Duration}, PartySize: {PartySize}", 
            dto.UserId, dto.TableId, dto.ReservationDateTime, dto.Duration, dto.PartySize);
        
        // Validate user exists
        var user = await unitOfWork.Users.GetByIdAsync(dto.UserId);
        if (user is null) 
        {
            logger.LogWarning("Reservation creation failed - User with ID {UserId} not found", dto.UserId);
            throw new Exception("User not found!");
        }
        logger.LogDebug("User validation passed for UserId: {UserId}", dto.UserId);
        
        // Validate table exists
        var table = await unitOfWork.Tables.GetByIdAsync(dto.TableId);
        if (table == null) 
        {
            logger.LogWarning("Reservation creation failed - Table with ID {TableId} not found", dto.TableId);
            throw new Exception($"Table with ID {dto.TableId} not found");
        }
        
        logger.LogDebug("Table validation passed - Table {TableNumber} (ID: {TableId}, Capacity: {Capacity})", 
            table.TableNumber, table.Id, table.Capacity);
        
        // Check time slot availability
        var dayOfWeek = dto.ReservationDateTime.DayOfWeek;
        logger.LogDebug("Checking time slot availability for {DayOfWeek}", dayOfWeek);
        
        var availableTimeSlot = await unitOfWork.TimeSlots.GetByDayOfWeekAsync(dayOfWeek);
        if (availableTimeSlot == null) 
        {
            logger.LogWarning("Reservation creation failed - No time slot available for {DayOfWeek}", dayOfWeek);
            throw new Exception("Timeslot for given week day doesn't exist!");
        }
        
        var availableTimeSlotStart = availableTimeSlot.StartTime;
        var availableTimeSlotEnd = availableTimeSlot.EndTime;
        logger.LogDebug("Available time slot: {StartTime} - {EndTime} on {DayOfWeek}", 
            availableTimeSlotStart, availableTimeSlotEnd, dayOfWeek);
        
        // Validate reservation fits within time slot
        var wantedTimeSlotStart = TimeOnly.FromDateTime(dto.ReservationDateTime);
        var wantedTimeSlotEnd = TimeOnly.FromDateTime(dto.ReservationDateTime.AddHours(dto.Duration));
        
        logger.LogDebug("Requested time slot: {RequestedStart} - {RequestedEnd}", 
            wantedTimeSlotStart, wantedTimeSlotEnd);

        var fitsTheTimeSlot = wantedTimeSlotStart >= availableTimeSlotStart && wantedTimeSlotEnd <= availableTimeSlotEnd;
        
        if (!fitsTheTimeSlot)
        {
            logger.LogWarning("Reservation creation failed - Requested time {RequestedStart}-{RequestedEnd} falls outside available slot {AvailableStart}-{AvailableEnd} on {DayOfWeek}", 
                wantedTimeSlotStart, wantedTimeSlotEnd, availableTimeSlotStart, availableTimeSlotEnd, dayOfWeek);
            throw new ReservationHoursOutOfBoundException("Reservation falls out of available time slot!");
        }
        
        logger.LogDebug("Time slot validation passed");
        
        // Check table availability (overlapping reservations)
        logger.LogDebug("Checking for overlapping reservations on table {TableId}", dto.TableId);
        var hasOverlappingReservation = await unitOfWork.Reservations.HasOverlappingReservationAsync(dto.ReservationDateTime, dto.Duration, dto.TableId);
        
        if (hasOverlappingReservation) 
        {
            logger.LogWarning("Reservation creation failed - Table {TableId} has overlapping reservation for {DateTime} duration {Duration}h", 
                dto.TableId, dto.ReservationDateTime, dto.Duration);
            throw new UnavailableTableException("Table is not available for the requested time slot!");
        }
        
        logger.LogDebug("No overlapping reservations found for table {TableId}", dto.TableId);
        
        var reservation = dto.ToEntity();
        logger.LogDebug("Saving reservation to database");
        
        await unitOfWork.Reservations.AddAsync(reservation);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Reservation created successfully - ID: {ReservationId}, User: {UserId}, Table: {TableId}, DateTime: {DateTime}", 
            reservation.Id, dto.UserId, dto.TableId, dto.ReservationDateTime);
        
        return reservation.ToDto();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete reservation with ID: {ReservationId}", id);
        
        if (id <= 0) 
        {
            logger.LogWarning("Delete reservation failed - Invalid ID provided: {ReservationId}", id);
            throw new BadRequestException("Given Id must be greater than zero!");
        }
        
        var reservation = await unitOfWork.Reservations.GetByIdAsync(id);
        if (reservation == null) 
        {
            logger.LogWarning("Delete reservation failed - Reservation with ID {ReservationId} not found", id);
            return false;
        }
        
        logger.LogDebug("Found reservation to delete - ID: {ReservationId}, DateTime: {DateTime}, Status: {Status}", 
            reservation.Id, reservation.ReservationDateTime, reservation.Status);
        
        var timeUntilReservation = reservation.ReservationDateTime - DateTime.Now;
        if (timeUntilReservation <= TimeSpan.FromHours(1))
        {
            logger.LogWarning("Delete reservation failed - Cannot cancel reservation {ReservationId} scheduled for {DateTime} (less than 1 hour notice)", 
                id, reservation.ReservationDateTime);
            throw new InvalidOperationException("Cannot cancel reservation less than 1 hour before scheduled time");
        }
        
        try
        {
            unitOfWork.Reservations.Delete(reservation);
            await unitOfWork.SaveChangesAsync();
            
            logger.LogInformation("Reservation deleted successfully - ID: {ReservationId}, was scheduled for {DateTime}", 
                id, reservation.ReservationDateTime);
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting reservation with ID: {ReservationId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        logger.LogInformation("Retrieving all reservations");
        
        try
        {
            var reservations = await unitOfWork.Reservations.GetAllAsync();
            var reservationDtos = reservations.ToDtos();
            
            logger.LogInformation("Successfully retrieved {Count} reservations", reservationDtos.Count());
            return reservationDtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all reservations");
            throw;
        }
    }
    
    public async Task<IEnumerable<ReservationDto>> GetAllByUserIdAsync(int userId)
    {
        logger.LogInformation("Retrieving all reservations for user ID: {UserId}", userId);
    
        if (userId <= 0)
        {
            logger.LogWarning("Invalid user ID provided: {UserId}", userId);
            throw new BadRequestException("User ID must be greater than zero");
        }
    
        try
        {
            var user = await unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found", userId);
                throw new Exception($"User with ID {userId} not found");
            }
        
            logger.LogDebug("User validation passed for UserId: {UserId}", userId);
        
            var reservations = await unitOfWork.Reservations.GetAllByUserIdAsync(userId);
            var reservationDtos = reservations.ToDtos();
        
            logger.LogInformation("Successfully retrieved {Count} reservations for user ID: {UserId}", 
                reservationDtos.Count(), userId);
        
            return reservationDtos;
        }
        catch (Exception ex) when (!(ex is BadRequestException))
        {
            logger.LogError(ex, "Error occurred while retrieving reservations for user ID: {UserId}", userId);
            throw;
        }
    }
    
}