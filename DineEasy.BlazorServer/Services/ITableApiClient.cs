using DineEasy.Application.DTOs.Table;
using DineEasy.SharedKernel.Models.Table;

public interface ITableApiClient
{
    Task<IEnumerable<TableDto>> GetAllTablesAsync();
    Task<TableDto?> GetTableByIdAsync(int id);
    Task<TableDto?> CreateTableAsync(CreateTableDto dto);
    Task<bool> UpdateTableAsync(int id, UpdateTableDto dto);
    Task<bool> DeleteTableAsync(int id);
}