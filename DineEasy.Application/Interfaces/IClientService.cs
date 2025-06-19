using DineEasy.Application.DTOs.Customer;

namespace DineEasy.Application.Interfaces;

public interface IClientService
{
    Task<ClientDto?> GetByIdAsync(int id);
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<bool> DeleteAsync(int id);
}