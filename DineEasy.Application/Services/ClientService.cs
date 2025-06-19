using DineEasy.Application.DTOs.Customer;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Interfaces;

namespace DineEasy.Application.Services;

public class ClientService : IClientService
{
    
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        var customer = await _unitOfWork.Clients.GetByIdAsync(id);
        return customer?.ToDto();
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        var customer = dto.ToEntity();

        await _unitOfWork.Clients.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return customer.ToDto();
    } 

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _unitOfWork.Clients.GetByIdAsync(id);

        if (customer == null) return false;

        _unitOfWork.Clients.Delete(customer);
        
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}