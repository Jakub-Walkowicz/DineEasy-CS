using DineEasy.Application.DTOs.Table;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Interfaces;
using DineEasy.Application.Extensions;
using Kiosk.WebAPI.Db.Exceptions;

namespace DineEasy.Application.Services;

public class TableService(IUnitOfWork unitOfWork) : ITableService
{
    public async Task<TableDto?> GetByIdAsync(int id)
    {
        if (id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
    
        var table = await unitOfWork.Tables.GetByIdAsync(id);
    
        return table?.ToDto();
    }

    public async Task<IEnumerable<TableDto>> GetAllAsync()
    {
        var tables = await unitOfWork.Tables.GetAllAsync();
        return tables.ToDtos();
    }

    public async Task<TableDto> CreateAsync(CreateTableDto dto)
    {
        var table = dto.ToEntity();
        
        await unitOfWork.Tables.AddAsync(table);
        await unitOfWork.SaveChangesAsync();
        return table.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
        
        var table = await unitOfWork.Tables.GetByIdAsync(id);

        if (table == null) return false;
        
        unitOfWork.Tables.Delete(table);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateAsync(UpdateTableDto dto)
    {
        if (dto.Id <= 0) throw new BadRequestException("Given Id must be greater than zero!");
    
        var existingTable = await unitOfWork.Tables.GetByIdAsync(dto.Id);
        if (existingTable == null) return false;
        
        existingTable.Capacity = dto.Capacity;
        existingTable.IsActive = dto.IsActive;
        existingTable.TableNumber = dto.TableNumber;
    
        unitOfWork.Tables.Update(existingTable);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}