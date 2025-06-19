using DineEasy.Application.DTOs.Table;

namespace DineEasy.Application.Interfaces;

public interface ITableService
{
    Task<TableDto?> GetByIdAsync(int id);
    Task<IEnumerable<TableDto>> GetAllAsync();
    Task<TableDto> CreateAsync(CreateTableDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(UpdateTableDto updateDto);
}