using DineEasy.Application.DTOs.Table;

namespace DineEasy.Application.Interfaces;

public interface ITableService
{
    Task<List<TableDto>> GetAllAsync();
    Task<TableDto> CreateAsync(CreateTableDto dto);
    Task<bool> DeleteAsync(int id);
}