using DineEasy.Application.DTOs.Customer;
using DineEasy.Application.Extensions;
using DineEasy.Application.Interfaces;
using DineEasy.Domain.Interfaces;

namespace DineEasy.Application.Services;

public class CustomerService : ICustomerService
{
    
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = dto.ToEntity();

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return customer.ToDto();
    } 

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);

        if (customer == null) return false;

        _unitOfWork.Customers.Delete(customer);
        
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}