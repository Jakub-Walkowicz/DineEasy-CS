using DineEasy.Application.DTOs.Table;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Extensions;

public static class TableExtensions
{
    public static TableDto ToDto(this Table table)
    {
        return new TableDto
        {
            Id = table.Id,
            Capacity = table.Capacity,
            IsActive = table.IsActive,
            TableNumber = table.TableNumber
        };
    }

    public static Table ToEntity(this CreateTableDto dto)
    {
        return new Table
        {
            Capacity = dto.Capacity,
            IsActive = dto.IsActive,
            TableNumber = dto.TableNumber
        };
    }
    
    public static IEnumerable<TableDto> ToDtos(this IEnumerable<Table> tables)
    {
        return tables.Select(table => table.ToDto()).ToList();
    }
    
}