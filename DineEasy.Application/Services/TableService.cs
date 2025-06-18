using DineEasy.Application.DTOs.Table;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Interfaces;
using DineEasy.Application.Extensions;

namespace DineEasy.Application.Services;

public class TableService : ITableService
{
    
    private readonly IUnitOfWork _unitOfWork;

    public TableService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<TableDto>> GetAllAsync()
    {
        var tables = await _unitOfWork.Tables.GetAllAsync();
        return tables.ToDtos();
    }

    public async Task<TableDto> CreateAsync(CreateTableDto dto)
    {
        var table = dto.ToEntity();
        
        await _unitOfWork.Tables.AddAsync(table);
        await _unitOfWork.SaveChangesAsync();
        return table.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var table = await _unitOfWork.Tables.GetByIdAsync(id);
        
        if (table == null) return false;
        
        _unitOfWork.Tables.Delete(table);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}